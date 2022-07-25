using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class AccountantController : BaseController
    {
        private readonly PatientServices _patientServices;
        private readonly HealthTypeServices _healthServices;
        private readonly ExaminationObjectServices _examinationObjectServices;
        private readonly PriceServices _priceServices;

        public AccountantController(PatientServices patientServices, HealthTypeServices healthTypeServices, 
            ExaminationObjectServices examinationObjectServices, PriceServices priceServices)
        {
            _patientServices = patientServices;
            _healthServices = healthTypeServices;
            _examinationObjectServices = examinationObjectServices;
            _priceServices = priceServices;
        }
        public async Task<IActionResult> Index(string searchName, string searchType, string searchObject, DateTime searchStart,
            DateTime searchEnd, int pageIndex = 1, int pageSize = 5)
        {
            searchName = searchName ?? ""; //check null value: if null value = "", else value = value
            searchType = searchType ?? "";
            searchObject = searchObject ?? "";
            var validateTime = new DateTime(2020, 1, 1);
            searchStart = searchStart < validateTime ? validateTime : searchStart;
            searchEnd = searchEnd < validateTime ? new DateTime(2025, 1, 1) : searchEnd;
            var patients = await _patientServices.GetPatientsAsync(pageIndex, pageSize, searchName, searchType, searchObject, searchStart, searchEnd);
            var healthTypes = await _healthServices.GetHealthTypesAsync();
            ViewBag.HealthTypes = healthTypes.Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = x.Name
            });
            var exams = await _examinationObjectServices.GetExamObjectsAsync();
            ViewBag.ExamObjects = exams.Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = x.Name
            });
            var prevKeyword = new
            {
                name = searchName,
                type = searchType,
                obj = searchObject,
                start = searchStart,
                end = searchEnd
            };
            ViewBag.PrevKeyword = searchName;
            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var patient = await _patientServices.GetPatientAsync(id);
            return View(patient);
        }

        public async Task<IActionResult> ChangePaid(string id)
        {
            await _patientServices.ChangePaidStatus(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ManagePrice()
        {
            var prices = await _priceServices.GetTablePricesAsync();
            return View(prices);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePrice(string name, int price)
        {
            var tablePrice = new TablePrice()
            {
                Name = name,
                Price = price
            };
            await _priceServices.CreateAsync(tablePrice);
            return RedirectToAction("ManagePrice");
        }
        public async Task<IActionResult> EditPrice(int id, string name, int price)
        {
            await _priceServices.EditAsync(id, price, name);
            return RedirectToAction("ManagePrice");
        }
        [HttpGet]
        public async Task<JsonResult> DetailsJson(int id)
        {
            var exam = await _priceServices.GetPriceAsync(id);
            var result = new
            {
                id = exam.Id,
                name = exam.Name,
                price = exam.Price
            };
            return Json(result);
        }
    }
    
}
