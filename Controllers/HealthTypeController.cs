using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class HealthTypeController : Controller
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
    }
}
