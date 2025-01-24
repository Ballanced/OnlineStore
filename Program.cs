using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineStore.Models;
using OnlineStore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { 
    options.SignIn.RequireConfirmedAccount = true; 
    }).AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSession();
builder.Services.AddControllersWithViews();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
