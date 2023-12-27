using demo.Models;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = "server=hms-db-hms-db.a.aivencloud.com;port=11793;user=avnadmin;password=AVNS_FZs-lAOyU4XtmPIr-FH;database=hms-db-new";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));
builder.Services.AddDbContext<HMSEntites>(optionBuilder => {
    optionBuilder.UseMySql(connectionString, serverVersion);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
AddEntityFrameworkStores<HMSEntites>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
