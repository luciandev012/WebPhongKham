using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;
using Bcrypt = BCrypt.Net.BCrypt;

namespace WebPhongKham.Services
{
    public class UserServices
    {
        private readonly MedicalDbContext _context;
        
        public UserServices(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync() => await _context.Users.ToListAsync();

        public async Task<User> GetUserAsync(string id) => await _context.Users.FindAsync(id);

        public async Task CreateUserAsync(User user)
        {
            var salt = Bcrypt.GenerateSalt(10);
            user.Password = Bcrypt.HashPassword(user.Password, salt);
            user.Id = Guid.NewGuid().ToString();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(string id, User user)
        {
            var res = await _context.Users.FindAsync(id);
            res.Address = user.Address;
            res.PhoneNumber = user.PhoneNumber;
            res.Email = user.Email;
            res.FullName = user.FullName;
            await _context.SaveChangesAsync();
        }
        
        public async Task ChangeUserStatus(string id)
        {
            var res = await _context.Users.FindAsync(id);
            res.Status = !res.Status;
            await _context.SaveChangesAsync();
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.Where(x => x.UserName == username && x.Status).FirstOrDefaultAsync();
            if(user != null && Bcrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task<bool> ChangePassAcsync(string id, string oldPass, string newPass)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null && Bcrypt.Verify(oldPass, user.Password))
            {
                var salt = Bcrypt.GenerateSalt(10);
                user.Password = Bcrypt.HashPassword(newPass, salt);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
