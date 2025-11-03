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
else
{
    app.UseExceptionHandler("/Error/ServerError");
    app.UseStatusCodePagesWithReExecute("/Error/StatusCode", "?code={0}");
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
