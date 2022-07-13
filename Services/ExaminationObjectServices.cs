using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;

namespace WebPhongKham.Services
{
    public class ExaminationObjectServices
    {
        private readonly MedicalDbContext _context;

        public ExaminationObjectServices(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExaminationObject>> GetExamObjectsAsync() => await _context.ExaminationObjects.ToListAsync();

        public async Task<ExaminationObject> GetAsync(string id)
        {
            return await _context.ExaminationObjects.FindAsync(id);
        }

        public async Task CreateAsync(ExaminationObject exam)
        {
            exam.Id = Guid.NewGuid().ToString();
            await _context.ExaminationObjects.AddAsync(exam);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var ht = await _context.ExaminationObjects.FindAsync(id);
            _context.ExaminationObjects.Remove(ht);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(string name, string id)
        {
            var ht = await _context.ExaminationObjects.FindAsync(id);
            ht.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePriceAsync(string id, float price)
        {
            var ht = await _context.ExaminationObjects.FindAsync(id);
            ht.Price = price;
            await _context.SaveChangesAsync();
        }
        public async Task<float> GetPriceAsync(string name) => (await _context.ExaminationObjects.Where(x => x.Name == name).FirstOrDefaultAsync()).Price;
    }
}
