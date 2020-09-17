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
    public class FarmController : Controller
    {
        private readonly HttpClient _httpClient;

        public FarmController()
        {
            _httpClient = new HttpClient();
        }

        // GET: Farm
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5005/api/farms/");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Farm>>(data);

                return View(list);
            }

            return RedirectToAction("Index");
        }

        // GET: Farm/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/farms/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Farm>(data);

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: Farm/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Farm/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Farm farm)
        {
            string json = JsonConvert.SerializeObject(farm);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = 
                    await _httpClient.PostAsync("http://localhost:5005/api/farms/", content);
                
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: Farm/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/farms/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<Farm>(data);

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // POST: Farm/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Farm farm)
        {
            farm.FarmId = id;
            string json = JsonConvert.SerializeObject(farm);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.PutAsync($"http://localhost:5005/api/farms/{farm.FarmId}", content);
                
                 if (result.IsSuccessStatusCode)
                 {
                     return RedirectToAction("Index");
                 }
            }
            
            ModelState.AddModelError("", "Error while creating data.");
            return View();
        }

        // GET: Farm/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5005/api/farms/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Farm>(data);

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // POST: Farm/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            string json = JsonConvert.SerializeObject(id);
            using (var content = new StringContent
                (json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result =
                    await _httpClient.DeleteAsync($"http://localhost:5005/api/farms/{id}");
                
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