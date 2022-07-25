using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using WebPhongKham.Services;

namespace WebPhongKham.Controllers
{
    public class XrayController : BaseController
    {
        private readonly PatientServices _patientServices;
        private readonly HealthTypeServices _healthServices;
        private readonly ExaminationObjectServices _examinationObjectServices;

        public XrayController(PatientServices patientServices, HealthTypeServices healthTypeServices, ExaminationObjectServices examinationObjectServices)
        {
            _patientServices = patientServices;
            _healthServices = healthTypeServices;
            _examinationObjectServices = examinationObjectServices;
        }
        public async Task<IActionResult> Index(string searchName, string searchType, string searchObject, DateTime searchStart,
            DateTime searchEnd,bool doneXray, int pageIndex = 1, int pageSize = 5)
        {
            searchName = searchName ?? ""; //check null value: if null value = "", else value = value
            searchType = searchType ?? "";
            searchObject = searchObject ?? "";
            var validateTime = new DateTime(2020, 1, 1);
            searchStart = searchStart < validateTime ? validateTime : searchStart;
            searchEnd = searchEnd < validateTime ? new DateTime(2025, 1, 1) : searchEnd;
            var patients = await _patientServices.GetPaidAndXrayPatientsAsync(pageIndex, pageSize, searchName, searchType, searchObject, searchStart, searchEnd, doneXray);
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
        public async Task<IActionResult> ChangeXray(string id)
        {
            await _patientServices.ChangeXrayStatus(id);
            return RedirectToAction("Index");
        }
        public async Task ExportExcel(string exam, string type, DateTime start, DateTime end, bool doneXray)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            exam = exam ?? "";
            type = type ?? "";
            var patients = await _patientServices.GetPatientsXrayForExportAsync(exam, type, start, end, doneXray);
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
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
            float totalPrice = 0;
            foreach (var patient in patients)
            {
                sheet.Cells[$"B{index}"].Value = patient.FullName;
                sheet.Cells[$"C{index}"].Value = patient.IdentityCode;
                sheet.Cells[$"D{index}"].Value = patient.DigitalInfo;
                sheet.Cells[$"E{index}"].Value = patient.DoB.ToString("dd-MM-yyyy");
                sheet.Cells[$"F{index}"].Value = patient.DoE.ToString("dd-MM-yyyy");
                sheet.Cells[$"G{index}"].Value = patient.HealthType;
                sheet.Cells[$"H{index}"].Value = patient.ExamObject;
                sheet.Cells[$"I{index}"].Value = String.Format("{0:n0}", patient.Total) + " VNĐ"; totalPrice += patient.Total;
                var res = patient.IsPaid ? "Đã thu tiền" : "Chưa thu tiền"; res += Environment.NewLine;
                res += patient.IsTest ? patient.IsDoneTest ? "Đã xét nghiệm" : "Chưa xét nghiệm" : "Không xét nghiệm"; res += Environment.NewLine;
                res += patient.IsXray ? patient.IsDoneXray ? "Đã chụp X-quang" : "Chưa chụp X-quang" : "Không chụp X-quang"; res += Environment.NewLine;
                sheet.Cells[$"J{index}"].Value = res;
                index++;
            }
            sheet.Cells[$"I{index}"].Value = "Tổng tiền: " + String.Format("{0:n0}", totalPrice) + " VNĐ";
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
