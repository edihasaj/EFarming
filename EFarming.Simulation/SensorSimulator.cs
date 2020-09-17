using EFarming.Models;
using System;

namespace EFarming.Simulation
{
    public class SensorSimulator
    {
        public double Get(SensorType sensorType)
        {
            double randomDouble = 0;

            switch (sensorType)
            {
                case SensorType.Humidity:
                    randomDouble = GetRandomNumber(70, 100);
                    break;
                case SensorType.Temperature:
                    randomDouble = GetRandomNumber(15, 20);
                    break;
                case SensorType.Moisture:
                    randomDouble = GetRandomNumber(800, 1000);
                    break;
            }

            return randomDouble;
        }

        public static double GetSignal()
        {
            return 0.0;
        }

        public static string GetLocation()
        {
            return "";
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}