using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebPhongKham.Models;
using WebPhongKham.Models.Entity;
using WebPhongKham.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
});
var connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<MedicalDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<HealthTypeServices>();
builder.Services.AddTransient<ExaminationObjectServices>();
builder.Services.AddTransient<PatientServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
