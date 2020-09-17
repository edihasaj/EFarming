using EFarming.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EFarming.Web.Controllers
{
    public class IrrigationModesController : Controller
    {
        private readonly HttpClient _httpClient;
        const string baseUrlApi = "http://localhost:5005/api";

        public IrrigationModesController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ActionResult> IrrigationMode(int id, int mode)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/irrigationModes/" + id);
                var data = await response.Content.ReadAsStringAsync();
                var irrigationMode = JsonConvert.DeserializeObject<IrrigationMode>(data);

                if (mode == 1)
                    irrigationMode.Mode = IrrigationModeEnum.Automatic;
                if (mode == 2)
                    irrigationMode.Mode = IrrigationModeEnum.Manual;

                string json = JsonConvert.SerializeObject(irrigationMode);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage updateResponse = 
                    await _httpClient.PutAsync($"http://localhost:5005/api/irrigationModes/{id}", content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            } 
            catch (Exception)
            {
                ModelState.AddModelError("", "Error while changing irrigation mode");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error while changing irrigation mode");
            return RedirectToAction("Index");
        }

        // GET: IrrigationModes
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/irrigationModes");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<IrrigationMode>>(data);

                return View(list);
            }

            return RedirectToAction("Index");
        }

        // GET: IrrigationModes/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/irrigationModes/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var mode = JsonConvert.DeserializeObject<IrrigationMode>(data);

                return View(mode);
            }

            return NotFound();
        }

        // GET: IrrigationModes/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: IrrigationModes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IrrigationMode irrigationMode)
        {
            string json = JsonConvert.SerializeObject(irrigationMode);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage result =
                    await _httpClient.PostAsync(baseUrlApi + "/irrigationModes", content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(irrigationMode);
            }
            catch
            {
                ModelState.AddModelError("", "Error while creating data.");
                return View(irrigationMode);
            }
        }

        // GET: IrrigationModes/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/irrigationModes/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var mode = JsonConvert.DeserializeObject<IrrigationMode>(data);

                return View(mode);
            }

            return NotFound();
        }

        // POST: IrrigationModes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IrrigationMode irrigationMode)
        {
            string json = JsonConvert.SerializeObject(irrigationMode);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage result =
                    await _httpClient.PutAsync(baseUrlApi + "/irrigationModes/" + id, content);

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(irrigationMode);
            }
            catch
            {
                ModelState.AddModelError("", "Error while creating data.");
                return View(irrigationMode);
            }
        }

        // GET: IrrigationModes/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUrlApi + "/irrigationModes/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var mode = JsonConvert.DeserializeObject<IrrigationMode>(data);

                return View(mode);
            }

            return NotFound();
        }

        // POST: IrrigationModes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IrrigationMode irrigationMode)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUrlApi + "/irrigationModes/" + id);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }
    }
}