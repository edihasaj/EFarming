using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EFarming.Models;
using EFarming.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EFarming.Web.Controllers
{
    public class ActuatorController : Controller
    {
        private readonly HttpClient _httpClient;

        public ActuatorController()
        {
            _httpClient = new HttpClient();
        }

        // GET: Actuator
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5005/api/actuators/");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<Actuator>>(data);

                foreach (var item in list)
                {
                    HttpResponseMessage zoneResponse = await _httpClient
                        .GetAsync($"http://localhost:5005/api/farmzones/{item.FarmZoneId}");

                    var zoneData = await zoneResponse.Content.ReadAsStringAsync();
                    item.FarmZone = JsonConvert.DeserializeObject<FarmZone>(zoneData);
                }
                
                return View(list);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // GET: Actuator/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/actuators/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<Actuator>(data);
                
                HttpResponseMessage zoneResponse = await _httpClient
                    .GetAsync($"http://localhost:5005/api/farmzones/"+model.FarmZoneId);
                
                var zoneData = await zoneResponse.Content.ReadAsStringAsync();
                model.FarmZone = JsonConvert.DeserializeObject<FarmZone>(zoneData);
                
                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // GET: Actuator/Create
        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5005/api/farmzones");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ViewBag.Zones = JsonConvert.DeserializeObject<List<FarmZone>>(data);
            }

            return View();
        }

        // POST: Actuator/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Actuator actuator)
        {
            string json = JsonConvert.SerializeObject(actuator);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.PostAsync("http://localhost:5005/api/actuators/", content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: Actuator/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage farmResponse = await _httpClient.GetAsync("http://localhost:5005/api/farmzones");

            if (farmResponse.IsSuccessStatusCode)
            {
                var data = await farmResponse.Content.ReadAsStringAsync();
                ViewBag.Zones = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FarmZone>>(data);
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/actuators/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Actuator>(data);

                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // POST: Actuator/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Actuator actuator)
        {
            string json = JsonConvert.SerializeObject(actuator);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.PutAsync($"http://localhost:5005/api/actuators/{actuator.Id}", content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: Actuator/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/actuators/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Actuator>(data);

                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // POST: Actuator/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            string json = JsonConvert.SerializeObject(id);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.DeleteAsync($"http://localhost:5005/api/actuators/{id}");

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