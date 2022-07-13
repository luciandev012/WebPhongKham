using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;

namespace WebPhongKham.Services
{
    public class HealthTypeServices
    {
        private readonly MedicalDbContext _context;

        public HealthTypeServices(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthType>> GetHealthTypesAsync() => await _context.HealthTypes.ToListAsync();

        public async Task CreateAsync(HealthType healthType)
        {
            healthType.Id = Guid.NewGuid().ToString();
            await _context.HealthTypes.AddAsync(healthType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id) 
        {
            var ht = await _context.HealthTypes.FindAsync(id);
            _context.HealthTypes.Remove(ht);
            await _context.SaveChangesAsync();
        }
        public async Task<HealthType> GetAsync(string id)
        {
            return await _context.HealthTypes.FindAsync(id);
        }
        public async Task EditAsync(string name, string id)
        {
            var ht = await _context.HealthTypes.FindAsync(id);
            ht.Name = name;
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePriceAsync(string id, float price)
        {
            var ht = await _context.HealthTypes.FindAsync(id);
            ht.Price = price;
            await _context.SaveChangesAsync();
        }
        public async Task<float> GetPriceAsync(string name) => (await _context.HealthTypes.Where(x => x.Name == name).FirstOrDefaultAsync()).Price;
    }
}
