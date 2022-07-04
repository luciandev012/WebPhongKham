using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly ExaminationObjectServices _examServices;

        public ExaminationController(ExaminationObjectServices examServices)
        {
            _examServices = examServices;
        }
        public async Task<IActionResult> Index()
        {
            var types = await _examServices.GetHealthTypesAsync();
            return View(types);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExaminationObject healthType)
        {
            try
            {
                await _examServices.CreateAsync(healthType);
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
                await _examServices.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }
    }
}
