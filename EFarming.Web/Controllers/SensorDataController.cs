using EFarming.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EFarming.Web.Controllers
{
    public class SensorDataController : Controller
    {
        private readonly HttpClient _httpClient;
        const string baseUrlApi = "http://localhost:5005/api";

        public SensorDataController()
        {
            _httpClient = new HttpClient();
        }

        // GET: SensorData
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/sensorData");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<SensorData>>(data);

                return View(list);
            }

            return RedirectToAction("Index");
        }

        // GET: SensorData/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/sensorData/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var sensorData = JsonConvert.DeserializeObject<SensorData>(data);

                return View(sensorData);
            }

            return NotFound();
        }

        // GET: SensorData/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SensorData/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SensorData sensorData)
        {
            string json = JsonConvert.SerializeObject(sensorData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage result =
                    await _httpClient.PostAsync(baseUrlApi + "/sensorData", content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(sensorData);
            }
            catch
            {
                ModelState.AddModelError("", "Error while creating data.");
                return View(sensorData);
            }
        }

        // GET: SensorData/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/sensorData/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var sensorData = JsonConvert.DeserializeObject<SensorData>(data);

                return View(sensorData);
            }

            return NotFound();
        }

        // POST: SensorData/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, SensorData sensorData)
        {
            string json = JsonConvert.SerializeObject(sensorData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage result =
                    await _httpClient.PutAsync(baseUrlApi + "/sensorData/" + id, content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(sensorData);
            }
            catch
            {
                ModelState.AddModelError("", "Error while creating data.");
                return View(sensorData);
            }
        }

        // GET: SensorData/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/sensorData/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var sensorData = JsonConvert.DeserializeObject<SensorData>(data);

                return View(sensorData);
            }

            return NotFound();
        }

        // POST: SensorData/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, SensorData sensorData)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUrlApi + "/sensorData/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }
    }
}
