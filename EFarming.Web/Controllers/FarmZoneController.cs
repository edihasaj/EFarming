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
    public class FarmZoneController : Controller
    {
        private readonly HttpClient _httpClient;

        public FarmZoneController()
        {
            _httpClient = new HttpClient();
        }
        
        // GET: FarmZone
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5005/api/farmzones/");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FarmZone>>(data);

                return View(list);
            }

            return RedirectToAction("Index");
        }

        // GET: FarmZone/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/farmzones/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<FarmZone>(data);
                
                return View(model);
            }
            
            return RedirectToAction("Index");
        }

        // GET: FarmZone/Create
        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5005/api/farms/");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ViewBag.Farms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Farm>>(data);
            }
            
            return View();
        }

        // POST: FarmZone/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FarmZone zone)
        {
            string json = JsonConvert.SerializeObject(zone);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = 
                    await _httpClient.PostAsync("http://localhost:5005/api/farmzones/", content);
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: FarmZone/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage farmResponse = await _httpClient.GetAsync("http://localhost:5005/api/farms/");

            if (farmResponse.IsSuccessStatusCode)
            {
                var data = await farmResponse.Content.ReadAsStringAsync();
                ViewBag.Farms = JsonConvert.DeserializeObject<List<Farm>>(data);
            }
            
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/farmzones/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FarmZone>(data);
                
                return View(model);
            }

            ModelState.AddModelError("", "Error while retrieving data.");
            return View();
        }

        // POST: FarmZone/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, FarmZone zone)
        {
            zone.ZoneId = id;
            string json = JsonConvert.SerializeObject(zone);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.PutAsync($"http://localhost:5005/api/farmzones/{zone.ZoneId}", content);
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: FarmZone/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/farmzones/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FarmZone>(data);
                
                return View(model);
            }

            ModelState.AddModelError("", "errorri");
            return View();
        }

        // POST: FarmZone/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            string json = JsonConvert.SerializeObject(id);
            using (var content = new StringContent
                  (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.DeleteAsync($"http://localhost:5005/api/farmzones/{id}");
                
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