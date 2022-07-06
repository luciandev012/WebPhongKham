using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientServices _patientServices;
        private readonly HealthTypeServices _healthServices;
        private readonly ExaminationObjectServices _examinationObjectServices;

        public PatientController(PatientServices patientServices, HealthTypeServices healthTypeServices, ExaminationObjectServices examinationObjectServices)
        {
            _patientServices = patientServices;
            _healthServices = healthTypeServices;
            _examinationObjectServices = examinationObjectServices;
        }

        public async Task<IActionResult> Index(string searchName, string searchType, string searchObject, DateTime searchStart,
            DateTime searchEnd, int pageIndex = 1, int pageSize = 2)
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

        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            patient.DoB = DateTime.SpecifyKind(patient.DoB, DateTimeKind.Utc);
            patient.DoE = DateTime.SpecifyKind(patient.DoE, DateTimeKind.Utc);
            await _patientServices.CreateAsync(patient);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var patient = await _patientServices.GetPatientAsync(id);
            var price = (await _examinationObjectServices.GetPriceAsync(patient.ExamObject)) + (await _healthServices.GetPriceAsync(patient.HealthType));
            ViewBag.Price = price;
            return View(patient);
        }

        [HttpGet]
        public async Task<JsonResult> DetailsJson(string id)
        {
            var patient = await _patientServices.GetPatientAsync(id);
            //string jsonString = JsonSerializer.Serialize(patient);
            //patient.DoB = patient.DoB.ToString("yyyy-MM-dd");
            var result = new 
            {
                id = patient.Id,
                fullName = patient.FullName,
                identityCode = patient.IdentityCode,
                doB = patient.DoB.ToString("yyyy-MM-dd"),
                doE = patient.DoE.ToString("yyyy-MM-dd"),
                examObject = patient.ExamObject,
                healthType = patient.HealthType,
                isPaid = patient.IsPaid,
                isTest = patient.IsTest,
                isXray = patient.IsXray,
                digitalInfo = patient.DigitalInfo
            };
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Patient patient)
        {
            patient.DoB = DateTime.SpecifyKind(patient.DoB, DateTimeKind.Utc);
            patient.DoE = DateTime.SpecifyKind(patient.DoE, DateTimeKind.Utc);
            await _patientServices.UpdateAsync(id, patient);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _patientServices.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
