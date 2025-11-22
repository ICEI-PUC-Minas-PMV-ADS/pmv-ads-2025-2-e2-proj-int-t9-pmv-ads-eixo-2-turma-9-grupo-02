using MedShare.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Configuração do DbContext com Sqlite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurações de Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // Esta lambda determina se é necessário o consentimento do usuário para cookies não essenciais.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Configuração da Autenticação por Cookie
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

// Configura o pipeline de requisição HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão de HSTS é 30 dias.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rota de fallback (CORREÇÃO):
// Permite acessar /ControllerName (ex: /MedicamentosInstituicao) e assume a Action Index por padrão.
app.MapControllerRoute(
    name: "controller_only_default_index",
    pattern: "{controller}",
    defaults: new { action = "Index" });

// Sua rota padrão original:
// Usada como fallback ou quando a Action é especificada (ex: /Auth/Login),
// mantendo Auth/ChooseType como a página inicial (se nenhum caminho for especificado).
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=ChooseType}/{id?}");

app.Run();