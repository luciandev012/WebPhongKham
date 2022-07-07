using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPhongKham.Models.Entity;
using Bcrypt = BCrypt.Net.BCrypt;

namespace WebPhongKham.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _usersCollection;
        
        public UserServices(IOptions<MedicalDatabaseSettings> medicalDB)
        {
            var mongoClient = new MongoClient(medicalDB.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(medicalDB.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>("users");
        }

        public async Task<List<User>> GetUsersAsync() => await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User> GetUserAsync(string id) => await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User user)
        {
            var salt = Bcrypt.GenerateSalt(10);
            user.Password = Bcrypt.HashPassword(user.Password, salt);
            await _usersCollection.InsertOneAsync(user);
        }
        public async Task UpdateUserAsync(string id, User user)
        {
            var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<User>.Update.Set(s => s.FullName, user.FullName)
                                              .Set(s => s.Address, user.Address)
                                              .Set(s => s.Email, user.Email)
                                              .Set(s => s.PhoneNumber, user.PhoneNumber)
                                              .Set(s => s.Role, user.Role);
            await _usersCollection.UpdateOneAsync(filter, update);                             
        }
        
        public async Task ChangeUserStatus(string id)
        {
            var user = await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<User>.Update.Set(s => s.Status, !user.Status);
            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _usersCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();
            if(user != null && Bcrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }
    }
}
