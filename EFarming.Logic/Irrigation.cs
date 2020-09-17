using EFarming.Models;
using EFarming.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EFarming.Logic
{
    public class Irrigate
    {
        const double sensorCheckDuration = 20000;
        const string baseUrlApi = "http://localhost:5005/api";
        IrrigationModeEnum irrigationMode = IrrigationModeEnum.Automatic;
        private IEnumerable<PredefinedValues> predefinedValues;
        static Timer timer;
        static readonly HttpClient client = new HttpClient();

        JsonSerializerSettings jsonOptions = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public Irrigate()
        {
            SetIrrigationMode(1, irrigationMode);
            CheckMode();
            SetTimer();
        }

        private async Task SetIrrigationMode(int farmZoneId, IrrigationModeEnum mode)
        {
            irrigationMode = mode;

            var modeDb = await GetIrrigationMode(farmZoneId);
            modeDb.Mode = mode;

            var json = JsonConvert.SerializeObject(modeDb, jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync(baseUrlApi + "/irrigationModes", data);
        }

        private async Task<IrrigationMode> GetIrrigationMode(int farmZoneId)
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/irrigationModes/" + farmZoneId);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IrrigationMode>(responseBody, jsonOptions);
        }

        private async Task CheckMode()
        {
            var mode = await GetIrrigationMode(1);
            irrigationMode = mode.Mode;

            var farmZones = await GetFarmZones();
            switch (irrigationMode)
            {
                case IrrigationModeEnum.Automatic:
                {
                    await CheckActuatorsInFarmZone(farmZones);
                    break;
                }
                case IrrigationModeEnum.Manual:
                {
                    await ManualModeActive(farmZones);
                    break;
                }
            }
        }

        private async Task CheckActuatorsInFarmZone(IEnumerable<FarmZone> farmZones)
        {
            foreach (var farmZone in farmZones)
            foreach (var actuator in farmZone.Actuators)
                await CalculateSensorValues(actuator);
        }

        private async Task ManualModeActive(IEnumerable<FarmZone> farmZones)
        {
            foreach (var farmZone in farmZones)
            foreach (var actuator in farmZone.Actuators)
                await ActuateValve(actuator.Id, true);
        }

        private async Task CalculateSensorValues(Actuator actuator)
        {
            if (actuator.Sensors == null || actuator.Sensors.Count < 1)
                return;

            predefinedValues = await GetPredefinedValues();

            bool isOpen = actuator.IsOpen;

            double humiditySensorsSignal = 0;
            double humidityMinValues = 0;
            double humidityPeakValues = 0;

            double moistureSensorsSignal = 0;
            double moistureMinValues = 0;
            double moisturePeakValues = 0;

            double tempSensorsSignal = 0;
            double tempMinValues = 0;
            double tempPeakValues = 0;

            foreach (var sensor in actuator.Sensors)
            {
                var sensorSignal = GetSensorSignal(sensor.Type);
                var sensorPredefinedValues = predefinedValues.FirstOrDefault(p => p.ValueFor == sensor.Type);
                var minValue = sensorPredefinedValues.Value - (sensorPredefinedValues.Value * 0.2);
                var peakValue = ((sensorPredefinedValues.MaxValue - sensorPredefinedValues.Value) / 4) +
                                sensorPredefinedValues.Value;

                switch (sensor.Type)
                {
                    case SensorType.Humidity:
                    {
                        if (humiditySensorsSignal > 0)
                            humiditySensorsSignal = (humiditySensorsSignal + sensorSignal) / 2;
                        else
                            humiditySensorsSignal = sensorSignal;

                        if (humidityMinValues == 0)
                        {
                            humidityMinValues = minValue;
                            humidityPeakValues = peakValue;
                        }

                        break;
                    }
                    case SensorType.Temperature:
                    {
                        if (tempSensorsSignal > 0)
                            tempSensorsSignal = (tempSensorsSignal + sensorSignal) / 2;
                        else
                            tempSensorsSignal = sensorSignal;

                        if (tempMinValues == 0)
                        {
                            tempMinValues = minValue;
                            tempPeakValues = peakValue;
                        }

                        break;
                    }
                    case SensorType.Moisture:
                    {
                        if (moistureSensorsSignal > 0)
                            moistureSensorsSignal = (moistureSensorsSignal + sensorSignal) / 2;
                        else
                            moistureSensorsSignal = sensorSignal;

                        if (moistureMinValues == 0)
                        {
                            moistureMinValues = minValue;
                            moisturePeakValues = peakValue;
                        }

                        break;
                    }
                }

                await AddSensorData(new SensorData
                {
                    SensorId = sensor.Id,
                    Signal = sensorSignal,
                    CreatedAt = DateTime.Now
                });
            }

            if (humiditySensorsSignal > humidityMinValues &&
                moistureSensorsSignal > moistureMinValues &&
                tempSensorsSignal <= tempMinValues)
            {
                await CloseValve(actuator.Id);
                isOpen = false;
            }
            else if (moistureSensorsSignal > moistureMinValues &&
                     moistureSensorsSignal <= moisturePeakValues &&
                     humiditySensorsSignal <= humidityPeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 50);
                isOpen = true;
            }
            else if (moistureSensorsSignal > moistureMinValues &&
                     moistureSensorsSignal <= moisturePeakValues &&
                     tempSensorsSignal <= tempPeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 50);
                isOpen = true;
            }
            else if (moistureSensorsSignal <= moisturePeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 50);
                isOpen = true;
            }
            else if (moistureSensorsSignal > moisturePeakValues &&
                     humiditySensorsSignal > humidityPeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 100);
                isOpen = true;
            }
            else if (moistureSensorsSignal > moisturePeakValues &&
                     tempSensorsSignal > tempPeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 100);
                isOpen = true;
            }
            else if (moistureSensorsSignal > moisturePeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 100);
                isOpen = true;
            }
            else if (humiditySensorsSignal > humidityPeakValues &&
                     tempSensorsSignal > tempPeakValues)
            {
                await ActuateValveWithFlowRate(actuator.Id, true, 100);
                isOpen = true;
            }
            else
            {
                await CloseValve(actuator.Id);
                isOpen = false;
            }

            if (actuator.IsOpen != isOpen)
            {
                actuator.IsOpen = isOpen;
                actuator.ValveOpenTime = DateTime.Now;
                await UpdateActuator(actuator);
            }
        }

        private async Task AddSensorData(SensorData sensorData)
        {
            var json = JsonConvert.SerializeObject(sensorData, jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync(baseUrlApi + "/sensorData", data);
        }

        private async Task ActuateValve(int actuatorId, bool valveOpen)
        {
            if (valveOpen)
            {
                ActuatorSimulator.OpenValve(actuatorId);
                await OpenValve(actuatorId);
            }
            else
            {
                ActuatorSimulator.CloseValve(actuatorId);
                await CloseValve(actuatorId);
            }
        }

        private async Task ActuateValveWithFlowRate(int actuatorId, bool valveOpen, double flowRate)
        {
            if (!valveOpen)
            {
                await CloseValve(actuatorId);
            }

            var actuator = await GetActuator(actuatorId);

            if (actuator.WaterFlowRate < flowRate)
            {
                await OpenValveWithFlowRate(actuatorId, flowRate);
            }
            else if (actuator.WaterFlowRate > flowRate)
            {
                await DecreaseValveFlowRate(actuatorId, flowRate);
            }
        }

        private async Task OpenValve(int id)
        {
            var actuator = await GetActuator(id);
            if (actuator == null)
                return;
            actuator.IsOpen = true;
            await UpdateActuator(actuator);
        }

        private async Task CloseValve(int id)
        {
            var actuator = await GetActuator(id);
            if (actuator == null)
                return;
            actuator.IsOpen = false;
            actuator.WaterFlowRate = 0;
            await UpdateActuator(actuator);
        }

        private async Task OpenValveWithFlowRate(int id, double flowRate)
        {
            var actuator = await GetActuator(id);
            if (actuator == null)
                return;
            actuator.IsOpen = true;
            actuator.WaterFlowRate = flowRate;
            await UpdateActuator(actuator);
        }

        private async Task DecreaseValveFlowRate(int id, double flowRate)
        {
            var actuator = await GetActuator(id);
            if (actuator == null)
                return;

            if (actuator.WaterFlowRate <= 5)
                actuator.IsOpen = false;
            else if (flowRate <= 5)
                actuator.IsOpen = false;
            else
                actuator.IsOpen = true;

            actuator.WaterFlowRate = flowRate;
            await UpdateActuator(actuator);
        }

        private void SetTimer()
        {
            timer = new Timer(sensorCheckDuration);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            CheckMode();
        }

        private async Task<Farm> GetFarm(int id)
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/farms/" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Farm>(responseBody, jsonOptions);
        }

        private async Task<IEnumerable<Farm>> GetFarms()
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/farms");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Farm>>(responseBody, jsonOptions);
        }

        private async Task<FarmZone> GetFarmZone(int id)
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/farmZones/" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FarmZone>(responseBody, jsonOptions);
        }

        private async Task<IEnumerable<FarmZone>> GetFarmZones()
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/farmZones");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<FarmZone>>(responseBody, jsonOptions);
        }

        private async Task<Actuator> GetActuator(int id)
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/actuators/" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Actuator>(responseBody, jsonOptions);
        }

        private async Task<IEnumerable<PredefinedValues>> GetPredefinedValues()
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/predefinedValues");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<PredefinedValues>>(responseBody, jsonOptions);
        }

        private double GetSensorSignal(SensorType sensorType)
        {
            var simulator = new SensorSimulator();
            return simulator.Get(sensorType);
        }

        private async Task<IEnumerable<Actuator>> GetActuators()
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/actuators");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Actuator>>(responseBody, jsonOptions);
        }

        private async Task SetValveCondition(int id, bool isGoodCondition)
        {
            var actuator = await GetActuator(id);
            if (actuator == null)
                return;
            actuator.IsGoodCondition = isGoodCondition;
            await UpdateActuator(actuator);
        }

        private async Task<Sensor> GetSensor(int id)
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/sensors/" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Sensor>(responseBody, jsonOptions);
        }

        private async Task<IEnumerable<Sensor>> GetSensors()
        {
            HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/sensors");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Sensor>>(responseBody, jsonOptions);
        }

        private async Task<Tuple<double, double>> GetFarmZoneLocation(int id)
        {
            var zone = await GetFarmZone(id);
            return Tuple.Create(zone.Latitude, zone.Longitude);
        }

        private async Task GetActuatorStateFromStore(int actuatorId)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(baseUrlApi + "/states/" + actuatorId);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var state = JsonConvert.DeserializeObject<State>(responseBody, jsonOptions);
                //CheckActuatorMode(state);
            }
            catch (HttpRequestException e)
            {
                // handle error
            }
        }

        private async Task<Models.State> SetActuatorStateInTheStore(int actuatorId, bool isOpen)
        {
            var state = new Models.State
            {
                ActuatorId = actuatorId,
                OpenDate = DateTime.Now,
                IsOpen = isOpen
            };

            var json = JsonConvert.SerializeObject(state, jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrlApi + "/states", data);

            string resultString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<State>(resultString, jsonOptions);
        }

        // private void CheckActuatorMode(State state)
        // {
        //     if (state.IsOpen)
        //     {
        //
        //     }
        // }

        private async Task UpdateActuator(Actuator actuator)
        {
            var json = JsonConvert.SerializeObject(actuator, jsonOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync(baseUrlApi + "/actuators/" + actuator.Id, data);
        }

        
    }
}