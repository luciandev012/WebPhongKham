using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using System;

namespace WebPhongKham.Controllers
{
    public class PatientController : BaseController
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
            patient.FullName = patient.FullName ?? "";
            patient.IdentityCode = patient.IdentityCode ?? "";
            await _patientServices.UpdateAsync(id, patient);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _patientServices.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task ExportExcel(string exam, string type, DateTime start, DateTime end)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            exam = exam ?? "";
            type = type ?? "";
            var patients = await _patientServices.GetPatientsForExportAsync(exam, type, start, end);
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            //Format file
            //sheet.Cells["B5:K8"].Merge = true;
            //sheet.Cells["B5:L20"].Style.Font.Name = "Times New Roman"; sheet.Cells["B5:L20"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //sheet.Cells["B5"].Style.Font.Bold = true;
            //sheet.Cells["B5"].Style.Font.Size = 20;
            //sheet.Cells["B5"].Style.Fill.BackgroundColor.SetColor(Color.Blue);
            //sheet.Cells["B5"].Value = "Chi tiết người bệnh"; sheet.Cells["B5:K19"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //sheet.Cells["B5:K19"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //sheet.Cells["B5"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //sheet.Cells["B5:K19"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //sheet.Cells["B10:K19"].Style.Font.Size = 14;
            //sheet.Cells["B10:C10"].Merge = true; sheet.Cells["B10"].Value = "Họ tên";
            //sheet.Cells["D10:E10"].Merge = true; sheet.Cells["D10"].Value = patient.FullName;
            //sheet.Cells["B11:C11"].Merge = true; sheet.Cells["B11"].Value = "CMND";
            //sheet.Cells["D11:E11"].Merge = true; sheet.Cells["D11"].Value = patient.IdentityCode;
            //sheet.Cells["B12:C12"].Merge = true; sheet.Cells["B12"].Value = "Ngày sinh";
            //sheet.Cells["D12:E12"].Merge = true; sheet.Cells["D12"].Value = patient.DoB.ToString("dd-MM-yyyy");
            ////
            //sheet.Cells["B14:C15"].Merge = true; sheet.Cells["B14"].Value = "Ngày khám";
            //sheet.Cells["B16:C19"].Merge = true; sheet.Cells["B16"].Value = patient.DoE.ToString("dd-MM-yyyy");
            //sheet.Cells["D14:E15"].Merge = true; sheet.Cells["D14"].Value = "Loại khám";
            //sheet.Cells["D16:E19"].Merge = true; sheet.Cells["D16"].Value = patient.HealthType;
            //sheet.Cells["F14:G15"].Merge = true; sheet.Cells["F14"].Value = "Đối tượng";
            //sheet.Cells["F16:G19"].Merge = true; sheet.Cells["F16"].Value = patient.ExamObject;
            //sheet.Cells["H14:I15"].Merge = true; sheet.Cells["H14"].Value = "Thành tiền";
            //var price = (await _examinationObjectServices.GetPriceAsync(patient.ExamObject)) + (await _healthServices.GetPriceAsync(patient.HealthType));
            //sheet.Cells["H16:I19"].Merge = true; sheet.Cells["H16"].Value = String.Format("{0:n0}", price) + "VNĐ";
            //sheet.Cells["J14:K15"].Merge = true;
            //var res = patient.IsPaid ? "Đã thu tiền" : "Chưa thu tiền"; res += Environment.NewLine;
            //res += patient.IsTest ? patient.IsDoneTest ? "Đã xét nghiệm" : "Chưa xét nghiệm" : "Không xét nghiệm"; res += Environment.NewLine;
            //res += patient.IsXray ? patient.IsDoneXray ? "Đã chụp X-quang" : "Chưa chụp X-quang" : "Không chụp X-quang"; res += Environment.NewLine;
            //sheet.Cells["J16:K19"].Merge = true; sheet.Cells["J16"].Value = res;
            sheet.Cells["B2"].Value = "Họ tên";
            sheet.Cells["C2"].Value = "CMND";
            sheet.Cells["D2"].Value = "Thông tin số";
            sheet.Cells["E2"].Value = "Ngày sinh";
            sheet.Cells["F2"].Value = "Ngày khám";
            sheet.Cells["G2"].Value = "Loại khám";
            sheet.Cells["H2"].Value = "Đối tượng";
            sheet.Cells["I2"].Value = "Thành tiền";
            sheet.Cells["J2"].Value = "Trạng thái";
            int index = 3;
            foreach (var patient in patients)
            {
                sheet.Cells[$"B{index}"].Value = patient.FullName;
                sheet.Cells[$"C{index}"].Value = patient.IdentityCode;
                sheet.Cells[$"D{index}"].Value = patient.DigitalInfo;
                sheet.Cells[$"E{index}"].Value = patient.DoB.ToString("dd-MM-yyyy");
                sheet.Cells[$"F{index}"].Value = patient.DoE.ToString("dd-MM-yyyy");
                sheet.Cells[$"G{index}"].Value = patient.HealthType;
                sheet.Cells[$"H{index}"].Value = patient.ExamObject;
                var price = (await _examinationObjectServices.GetPriceAsync(patient.ExamObject)) + (await _healthServices.GetPriceAsync(patient.HealthType));
                sheet.Cells[$"I{index}"].Value = String.Format("{0:n0}", price) + "VNĐ";
                var res = patient.IsPaid ? "Đã thu tiền" : "Chưa thu tiền"; res += Environment.NewLine;
                res += patient.IsTest ? patient.IsDoneTest ? "Đã xét nghiệm" : "Chưa xét nghiệm" : "Không xét nghiệm"; res += Environment.NewLine;
                res += patient.IsXray ? patient.IsDoneXray ? "Đã chụp X-quang" : "Chưa chụp X-quang" : "Không chụp X-quang"; res += Environment.NewLine;
                sheet.Cells[$"J{index}"].Value = res;
                index++;
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            //var response = HttpContext.Current.Response;
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment; filename=" + "Report.xlsx");
            await Response.Body.WriteAsync(ep.GetAsByteArray());
            Response.Body.Close();
        }
    }
}
