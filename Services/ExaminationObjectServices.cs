using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebPhongKham.Models.Entity;

namespace WebPhongKham.Services
{
    public class ExaminationObjectServices
    {
        private readonly IMongoCollection<ExaminationObject> _examCollection;

        public ExaminationObjectServices(IOptions<MedicalDatabaseSettings> medicalDB)
        {
            var mongoClient = new MongoClient(medicalDB.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(medicalDB.Value.DatabaseName);

            _examCollection = mongoDatabase.GetCollection<ExaminationObject>("examinationObjects");
        }

        public async Task<List<ExaminationObject>> GetHealthTypesAsync() => await _examCollection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(ExaminationObject exam) => await _examCollection.InsertOneAsync(exam);

        public async Task DeleteAsync(string id) => await _examCollection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}
