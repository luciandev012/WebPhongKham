using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.Entity;
using WebPhongKham.Models.Entity;
using WebPhongKham.Models.Paging;

namespace WebPhongKham.Services
{
    public class PatientServices
    {
        private readonly IMongoCollection<Patient> _patientCollection;

        public PatientServices(IOptions<MedicalDatabaseSettings> medicalDB)
        {
            var mongoClient = new MongoClient(medicalDB.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(medicalDB.Value.DatabaseName);

            _patientCollection = mongoDatabase.GetCollection<Patient>("patients");
        }

        public async Task<PagedResult<Patient>> GetPatientsAsync(int pageIndex, int pageSize, string name, string type, string obj, DateTime start, DateTime end)
        {
            var res = await _patientCollection.Find(x => x.FullName.ToLower().Contains(name)
                                                        && x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(obj)).SortByDescending(x => x.DoE).ToListAsync();
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
            var res = await _patientCollection.Find(x => x.FullName.ToLower().Contains(name)
                                                        && x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(obj)
                                                        && x.IsPaid && x.IsTest).SortByDescending(x => x.DoE).ToListAsync();
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
            var res = await _patientCollection.Find(x => x.FullName.ToLower().Contains(name)
                                                        && x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(obj)
                                                        && x.IsPaid && x.IsXray).SortByDescending(x => x.DoE).ToListAsync();
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

        public async Task CreateAsync(Patient patient) => await _patientCollection.InsertOneAsync(patient);
        
        public async Task UpdateAsync(string id, Patient newPatient)
        {
            var filter = Builders<Patient>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Patient>.Update.Set(s => s.FullName, newPatient.FullName)
                                              .Set(s => s.IdentityCode, newPatient.IdentityCode)
                                              .Set(s => s.DoB, newPatient.DoB)
                                              .Set(s => s.DoE, newPatient.DoE)
                                              .Set(s => s.ExamObject, newPatient.ExamObject)
                                              .Set(s => s.HealthType, newPatient.HealthType)
                                              .Set(s => s.IsTest, newPatient.IsTest)
                                              .Set(s => s.IsXray, newPatient.IsXray)
                                              .Set(s => s.DigitalInfo, newPatient.DigitalInfo);
            await _patientCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id) => await _patientCollection.FindOneAndDeleteAsync(x => x.Id == id);

        public async Task<Patient> GetPatientAsync(string id) => await _patientCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task ChangePaidStatus(string id)
        {
            var patient = await _patientCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var filter = Builders<Patient>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Patient>.Update.Set(s => s.IsPaid, !patient.IsPaid);
            await _patientCollection.UpdateOneAsync(filter, update);
        }
        public async Task ChangeTestStatus(string id)
        {
            var patient = await _patientCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var filter = Builders<Patient>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Patient>.Update.Set(s => s.IsDoneTest, !patient.IsDoneTest);
            await _patientCollection.UpdateOneAsync(filter, update);
        }
        public async Task ChangeXrayStatus(string id)
        {
            var patient = await _patientCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var filter = Builders<Patient>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Patient>.Update.Set(s => s.IsDoneXray, !patient.IsDoneXray);
            await _patientCollection.UpdateOneAsync(filter, update);
        }
        public async Task<List<Patient>> GetPatientsForExportAsync(string exam, string type, DateTime start, DateTime end)
        {
            var res = await _patientCollection.Find(x => x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(exam)).SortByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            return patients.ToList();
        }
        public async Task<List<Patient>> GetPatientsTestForExportAsync(string exam, string type, DateTime start, DateTime end)
        {
            var res = await _patientCollection.Find(x => x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(exam) && x.IsPaid && x.IsTest).SortByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            return patients.ToList();
        }
        public async Task<List<Patient>> GetPatientsXrayForExportAsync(string exam, string type, DateTime start, DateTime end)
        {
            var res = await _patientCollection.Find(x => x.HealthType.ToLower().Contains(type)
                                                        && x.ExamObject.ToLower().Contains(exam) && x.IsPaid && x.IsXray).SortByDescending(x => x.DoE).ToListAsync();
            var patients = from p in res
                           where p.DoE.CompareTo(start) != -1 && p.DoE.CompareTo(end) != 1
                           select p;
            return patients.ToList();
        }
    }
}
