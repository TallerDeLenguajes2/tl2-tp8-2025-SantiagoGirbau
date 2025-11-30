
using tl2_tp8_2025_SantiagoGirbau.Interfaces;
// aca van mas cosas segun el profe, pero aparentemente no hace tanta falta.
using tl2_tp8_2025_SantiagoGirbau.Controllers;
//using tl2_tp8_2025_SantiagoGirbau.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Sesión y Acceso a Contexto (CLAVE para la autenticación)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
 options.IdleTimeout = TimeSpan.FromMinutes(30);
 options.Cookie.HttpOnly = true;
 options.Cookie.IsEssential = true;
});
// Registro de la Inyección de Dependencia (TODOS AddScoped)
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepository>();
builder.Services.AddScoped<IUserRepository, UsuarioRepository>();
//builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSession();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
