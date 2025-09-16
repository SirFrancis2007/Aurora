using System.Data;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MySQL");

//builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnector(connectionString));
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));
//builder.Services.AddScoped<(aca va la interface), (Aca va la capa de datos que hereda la interface)>();
builder.Services.AddScoped<IRepoEmpresa, RepoEmpresa>();
builder.Services.AddScoped<IRepoAdministrador, RepoAdministrador>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(61);
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
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
