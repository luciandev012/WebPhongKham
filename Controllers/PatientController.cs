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

        public async Task<IActionResult> Index()
        {
            var patients = await _patientServices.GetPatientsAsync();
            var healthTypes = await _healthServices.GetHealthTypesAsync();
            ViewBag.HealthTypes = healthTypes.Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = x.Name
            });
            var exams = await _examinationObjectServices.GetHealthTypesAsync();
            ViewBag.ExamObjects = exams.Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = x.Name
            });
            return View(patients);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            await _patientServices.CreateAsync(patient);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var patient = await _patientServices.GetPatientAsync(id);
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
                doB = patient.DoB.ToString("dd/MM/yyyy"),
                doE = patient.DoE.ToString("dd/MM/yyyy"),
                examObject = patient.ExamObject,
                healthType = patient.HealthType,
                isPaid = patient.IsPaid,
                isTest = patient.IsTest,
                isXray = patient.IsXray
            };
            return Json(result);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _patientServices.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
