using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;
using WebPhongKham.Models.Paging;

namespace WebPhongKham.Services
{
    public class PatientServices
    {
        private readonly MedicalDbContext _context;

        public PatientServices(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Patient>> GetPatientsAsync(int pageIndex, int pageSize, string name, string type, string obj, DateTime start, DateTime end)
        {
            var res = await _context.Patients.Where(x => x.FullName.ToLower().Contains(name)
                                                        && x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(obj)).OrderByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            var totalRow = patients.Count();
            var result = patients.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var pagedResult = new PagedResult<Patient>
            {
                Items = result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }
        public async Task<PagedResult<Patient>> GetPaidAndTestPatientsAsync(int pageIndex, int pageSize, string name, string type, string obj, DateTime start, DateTime end)
        {
            var res = await _context.Patients.Where(x => x.FullName.ToLower().Contains(name)
                                                        && x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(obj)
                                                        && x.IsPaid && x.IsTest).OrderByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            var totalRow = patients.Count();
            var result = patients.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var pagedResult = new PagedResult<Patient>
            {
                Items = result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }
        public async Task<PagedResult<Patient>> GetPaidAndXrayPatientsAsync(int pageIndex, int pageSize, string name, string type, string obj, DateTime start, DateTime end)
        {
            var res = await _context.Patients.Where(x => x.FullName.ToLower().Contains(name)
                                                        && x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(obj)
                                                        && x.IsPaid && x.IsXray).OrderByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            var totalRow = patients.Count();
            var result = patients.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var pagedResult = new PagedResult<Patient>
            {
                Items = result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }

        public async Task CreateAsync(Patient patient)
        {
            patient.Id = Guid.NewGuid().ToString();
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(string id, Patient newPatient)
        {
            var patient = await _context.Patients.FindAsync(id);
            patient.FullName = newPatient.FullName;
            patient.IdentityCode = newPatient.IdentityCode;
            patient.DoB = newPatient.DoB;
            patient.DoE = newPatient.DoE;
            patient.ExamObject = newPatient.ExamObject;
            patient.HealthType = newPatient.HealthType;
            patient.IsTest = newPatient.IsTest;
            patient.IsXray = newPatient.IsXray;
            patient.DigitalInfo = newPatient.DigitalInfo;
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetPatientAsync(string id) => await _context.Patients.FindAsync(id);

        public async Task ChangePaidStatus(string id)
        {
            var patient = await _context.Patients.FindAsync(id);
            patient.IsPaid = !patient.IsPaid;
            await _context.SaveChangesAsync();
        }
        public async Task ChangeTestStatus(string id)
        {
            var patient = await _context.Patients.FindAsync(id);
            patient.IsDoneTest = !patient.IsDoneTest;
            await _context.SaveChangesAsync();
        }
        public async Task ChangeXrayStatus(string id)
        {
            var patient = await _context.Patients.FindAsync(id);
            patient.IsDoneXray = !patient.IsDoneXray;
            await _context.SaveChangesAsync();
        }
        public async Task<List<Patient>> GetPatientsForExportAsync(string exam, string type, DateTime start, DateTime end)
        {
            var res = await _context.Patients.Where(x => x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(exam)).OrderByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            return patients.ToList();
        }
        public async Task<List<Patient>> GetPatientsTestForExportAsync(string exam, string type, DateTime start, DateTime end)
        {
            var res = await _context.Patients.Where(x => x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(exam) && x.IsPaid && x.IsTest).OrderByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            return patients.ToList();
        }
        public async Task<List<Patient>> GetPatientsXrayForExportAsync(string exam, string type, DateTime start, DateTime end)
        {
            var res = await _context.Patients.Where(x => x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(exam) && x.IsPaid && x.IsXray).OrderByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            return patients.ToList();
        }
    }
}
