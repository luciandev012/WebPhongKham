using Microsoft.EntityFrameworkCore;
using WebPhongKham.Models.Entity;

namespace WebPhongKham.Models
{
    public class MedicalDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<HealthType> HealthTypes { get; set; }
        public DbSet<ExaminationObject> ExaminationObjects { get; set; }
        public DbSet<TablePrice> TablePrices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Connection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Patient>().HasKey(p => p.Id);
            builder.Entity<HealthType>().HasKey(h => h.Id);
            builder.Entity<ExaminationObject>().HasKey(e => e.Id);
            builder.Entity<TablePrice>().HasKey(e => e.Id);
            builder.Entity<User>().HasData(new User
            {
                FullName = "Admin", UserName = "admin", Password = "$2a$10$SXnrntO1/dUB/ysxj80vG.wMv6E6X8fNfPfHSNu8w2ZsBtiGINd0y",
                Address = "Hà Nội", Email = "admin@gmail.com", PhoneNumber = "0123456789", Role = "admin", Status = true, Id = Guid.NewGuid().ToString()
            });
            base.OnModelCreating(builder);
        }

    }
}
