using Microsoft.AspNetCore.Mvc;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class ExaminationController : BaseController
    {
        private readonly ExaminationObjectServices _examServices;

        public ExaminationController(ExaminationObjectServices examServices)
        {
            _examServices = examServices;
        }
        public async Task<IActionResult> Index()
        {
            var types = await _examServices.GetExamObjectsAsync();
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
        [HttpPost]
        public async Task<IActionResult> Edit(ExaminationObject healthType, string id)
        {
            try
            {
                await _examServices.EditAsync(healthType.Name, id);
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
        [HttpGet]
        public async Task<JsonResult> DetailsJson(string id)
        {
            var exam = await _examServices.GetAsync(id);
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
