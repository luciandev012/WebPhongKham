using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class AccountantController : BaseController
    {
        private readonly PatientServices _patientServices;
        private readonly HealthTypeServices _healthServices;
        private readonly ExaminationObjectServices _examinationObjectServices;

        public AccountantController(PatientServices patientServices, HealthTypeServices healthTypeServices, ExaminationObjectServices examinationObjectServices)
        {
            _patientServices = patientServices;
            _healthServices = healthTypeServices;
            _examinationObjectServices = examinationObjectServices;
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
        public async Task<IActionResult> ManageEOPrice()
        {
            var examObjects = await _examinationObjectServices.GetExamObjectsAsync();
            return View(examObjects);
        }
        [HttpPost]
        public async Task<IActionResult> ManageEOPrice(string id, float price)
        {
            await _examinationObjectServices.UpdatePriceAsync(id, price);
            return RedirectToAction("ManageEOPrice");
        }
        [HttpGet]
        public async Task<IActionResult> ManageHTPrice()
        {
            var healths = await _healthServices.GetHealthTypesAsync();
            return View(healths);
        }
        [HttpPost]
        public async Task<IActionResult> ManageHTPrice(string id, float price)
        {
            await _healthServices.UpdatePriceAsync(id, price);
            return RedirectToAction("ManageHTPrice");
        }
    }
}
