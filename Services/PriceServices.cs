using Microsoft.EntityFrameworkCore;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;

namespace WebPhongKham.Services
{
    public class PriceServices
    {
        private readonly MedicalDbContext _context;

        public PriceServices(MedicalDbContext context)
        {
            _context = context;
        }
        public async Task<List<TablePrice>> GetTablePricesAsync() => await _context.TablePrices.ToListAsync();

        public async Task CreateAsync(TablePrice price)
        {
            await _context.AddAsync(price);
            await _context.SaveChangesAsync();
        }
        public async Task<TablePrice> GetPriceAsync(int id)
        {
            return await _context.TablePrices.FindAsync(id);
        }
        public async Task EditAsync(int id, int price, string name)
        {
            var p = await _context.TablePrices.FindAsync(id);
            p.Price = price;
            p.Name = name;
            await _context.SaveChangesAsync();
        }
    }
}
