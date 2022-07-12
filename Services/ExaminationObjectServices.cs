using Microsoft.Extensions.Options;
using MongoDB.Bson;
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

        public async Task<List<ExaminationObject>> GetExamObjectsAsync() => await _examCollection.Find(_ => true).ToListAsync();

        public async Task<ExaminationObject> GetAsync(string id) => await _examCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ExaminationObject exam) => await _examCollection.InsertOneAsync(exam);

        public async Task DeleteAsync(string id) => await _examCollection.FindOneAndDeleteAsync(x => x.Id == id);

        public async Task EditAsync(string name, string id)
        {
            var filter = Builders<ExaminationObject>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<ExaminationObject>.Update.Set(s => s.Name, name);
            await _examCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdatePriceAsync(string id, float price)
        {
            var filter = Builders<ExaminationObject>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<ExaminationObject>.Update.Set(s => s.Price, price);
            await _examCollection.UpdateOneAsync(filter, update);
        }
        public async Task<float> GetPriceAsync(string name) => (await _examCollection.Find(x => x.Name == name).FirstOrDefaultAsync()).Price;
    }
}
