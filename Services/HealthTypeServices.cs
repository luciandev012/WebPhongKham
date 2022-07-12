using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPhongKham.Models.Entity;

namespace WebPhongKham.Services
{
    public class HealthTypeServices
    {
        private readonly IMongoCollection<HealthType> _healthCollection;

        public HealthTypeServices(IOptions<MedicalDatabaseSettings> medicalDB)
        {
            var mongoClient = new MongoClient(medicalDB.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(medicalDB.Value.DatabaseName);

            _healthCollection = mongoDatabase.GetCollection<HealthType>("healthTypes");
        }

        public async Task<List<HealthType>> GetHealthTypesAsync() => await _healthCollection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(HealthType healthType) => await _healthCollection.InsertOneAsync(healthType);

        public async Task DeleteAsync(string id) => await _healthCollection.FindOneAndDeleteAsync(x => x.Id == id);
        public async Task<HealthType> GetAsync(string id) => await _healthCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task EditAsync(string name, string id)
        {
            var filter = Builders<HealthType>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<HealthType>.Update.Set(s => s.Name, name);
            await _healthCollection.UpdateOneAsync(filter, update);
        }
        public async Task UpdatePriceAsync(string id, float price)
        {
            var filter = Builders<HealthType>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<HealthType>.Update.Set(s => s.Price, price);
            await _healthCollection.UpdateOneAsync(filter, update);
        }
        public async Task<float> GetPriceAsync(string name) => (await _healthCollection.Find(x => x.Name == name).FirstOrDefaultAsync()).Price;
    }
}
