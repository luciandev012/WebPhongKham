using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<PagedResult<Patient>> GetPatientsAsync(int pageIndex, int pageSize)
        {
            var patients = await _patientCollection.Find(_ => true).ToListAsync();
            var totalRow = patients.Count;
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
                                              .Set(s => s.IsXray, newPatient.IsXray);
            await _patientCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id) => await _patientCollection.FindOneAndDeleteAsync(x => x.Id == id);

        public async Task<Patient> GetPatientAsync(string id) => await _patientCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
