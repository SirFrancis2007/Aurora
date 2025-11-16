using System.Data;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));
builder.Services.AddScoped<IRepoEmpresa, RepoEmpresa>();
builder.Services.AddScoped<IRepoAdministrador, RepoAdministrador>();
builder.Services.AddScoped<IRepoConductor, RepoConductor>();
builder.Services.AddScoped<IRepoVehiculo, RepoVehiculo>();
builder.Services.AddScoped<IRepoVehiculoConductor, RepoVehiculoConductor>();
builder.Services.AddScoped<IRepoPedido, RepoPedido>();
builder.Services.AddScoped<IRepoHisrorialPedido, RepoHistorialPedido>();
builder.Services.AddScoped<IRepoRuta, RepoRuta>();

builder.Services.AddScoped<IRepoAutenticacion, RepoEmpresa>(); 
builder.Services.AddScoped<IRepoAutenticacion, RepoAdministrador>();
builder.Services.AddScoped<IRepoAutenticacion, RepoConductor>();
builder.Services.AddScoped<RepoAutenticar>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // tiempo máximo inactividad
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/Home/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(0.1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error/ServerError");
    app.UseStatusCodePagesWithReExecute("/Error/StatusCode", "?code={0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
