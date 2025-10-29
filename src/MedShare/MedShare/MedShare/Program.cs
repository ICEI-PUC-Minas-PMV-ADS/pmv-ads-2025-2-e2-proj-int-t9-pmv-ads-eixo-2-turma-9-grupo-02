using MedShare.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")),
//    ServiceLifetime.Scoped
//);

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "MedShareDatabase.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"),
    ServiceLifetime.Scoped
);

builder.Services.AddScoped<MedShare.Services.IDoacaoService, MedShare.Services.DoacaoService>();


builder.Services.Configure<CookiePolicyOptions>(options => 
{
    //This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
 });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => { 
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

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
    pattern: "{controller=Auth}/{action=ChooseType}/{id?}");

app.Run();
