using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EFarming.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EFarming.Web.Controllers
{
    public class SensorController : Controller
    {
        private readonly HttpClient _httpClient;

        public SensorController()
        {
            _httpClient = new HttpClient();
        }
        
        // GET: Sensor
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5005/api/sensors/");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Sensor>>(data);

                return View(list);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // GET: Sensor/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/sensors/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Sensor>(data);

                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // GET: Sensor/Create
        public ActionResult Create()
        {
            // var sensor = new Sensor
            // {
            //     SensorTypes = Enum.GetValues(typeof(SensorType)).Cast<SensorType>()
            // };
            
            return View();
        }

        // POST: Sensor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Sensor sensor)
        {
            string json = JsonConvert.SerializeObject(sensor);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = 
                    await _httpClient.PostAsync("http://localhost:5005/api/sensors/", content);
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: Sensor/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/sensors/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Sensor>(data);

                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // POST: Sensor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Sensor sensor)
        {
            string json = JsonConvert.SerializeObject(sensor);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.PutAsync($"http://localhost:5005/api/sensors/{sensor.Id}", content);
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: Sensor/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/sensors/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Sensor>(data);

                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // POST: Sensor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            string json = JsonConvert.SerializeObject(id);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.DeleteAsync($"http://localhost:5005/api/sensors/{id}");
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }
    }
}