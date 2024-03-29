﻿using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class HealthTypeController : BaseController
    {
        private readonly HealthTypeServices _healthTypeServices;

        public HealthTypeController(HealthTypeServices healthTypeServices)
        {
            _healthTypeServices = healthTypeServices;
        }
        public async Task<IActionResult> Index()
        {
            var types = await _healthTypeServices.GetHealthTypesAsync();
            return View(types);
        }
        [HttpPost]
        public async Task<IActionResult> Create(HealthType healthType)
        {
            try
            {
                await _healthTypeServices.CreateAsync(healthType);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string name, string id)
        {
            try
            {
                await _healthTypeServices.EditAsync(name, id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _healthTypeServices.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<JsonResult> DetailsJson(string id)
        {
            var exam = await _healthTypeServices.GetAsync(id);
            var result = new
            {
                id = exam.Id,
                name = exam.Name,
                deletable = exam.Deletable,
                price = exam.Price
            };
            return Json(result);
        }
    }
}
