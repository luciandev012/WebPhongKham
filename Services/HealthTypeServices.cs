using Microsoft.Extensions.Options;
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
    }
}
