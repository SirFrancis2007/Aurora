using System.ComponentModel.DataAnnotations;
using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
using MinimalAPI.DTO;
using MySqlConnector;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//CARGAR LA BASE DE DATOS EN LA Memoria RAM
var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

builder.Services.AddScoped<IRepoRuta, RepoRuta>();
builder.Services.AddScoped<IRepoEmpresa, RepoEmpresa>();
builder.Services.AddScoped<IRepoAdministrador, RepoAdministrador>();
builder.Services.AddScoped<IRepoConductor, RepoConductor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

// ------------------- ENTIDAD RUTA ---------------------------------- //

// METODO GET DE RUTAS

app.MapGet("/Ruta", async (IRepoRuta _repo) => await _repo.ObtenerAsync).WithTags("Ruta");

app.MapGet("/Ruta/{id}", async (int id, IRepoRuta _repo) =>
{
    var resultado = await _repo.DetalleAsync(id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
}).WithTags("Ruta");


// METODO POST DE RUTAS

app.MapPost("/Ruta", async (RutaDTO todo, IRepoRuta _repo) =>
{
    var Nueva_Ruta = new Ruta
    {
        IdRuta = 0,
        Origen = todo.Origen,
        Destino = todo.Destino
    };
    await _repo.AltaAsync(Nueva_Ruta);

    return Results.Created($"/Ruta/{todo}", todo);
}).WithTags("Ruta");

// ------------------- ENTIDAD EMPRESA ---------------------------------- //

//METODO GET PARA ENTIDAD EMPRESA
app.MapGet("/Empresa", async (IRepoEmpresa _repoempresa) =>
{
    var empresas = await _repoempresa.ObtenerAsync;
    return Results.Ok(empresas);
}).WithTags("Empresa");


app.MapGet("/Empresa/{id}", async (uint id, IRepoEmpresa _repoempresa) =>
{
    var resultado = await _repoempresa.ObtenerPedidosAsync((int)id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
}).WithTags("Empresa");

//METODO PARA ELIMINAR ENTIDAD EMPRESA
app.MapDelete("/Empresa/{id}", async (uint id, IRepoEmpresa _repoempresa) =>
{
    var empresa = await _repoempresa.DetalleAsync(id);
    if (empresa is not null)
    {
        await _repoempresa.EliminarAdministradorAsync((int)id);
        await _repoempresa.EliminarEmpresaAsync((int)id);
        return Results.NoContent();
    }

    return Results.NotFound();
}).WithTags("Empresa");


// METODO PARA CREAR EMPRESA
app.MapPost("/Empresa", async (EmpresaDTO dto, IRepoEmpresa _repoempresa) =>
{
    var empresa = new Empresa
    {
        Nombre = dto.Nombre
    };

    await _repoempresa.AltaAsync(empresa);

    var empresaDto = new EmpresaDTO
    {
        Nombre = empresa.Nombre
    };

    return Results.Created($"/Empresa/{empresa.IdEmpresa}", empresaDto);
}).WithTags("Empresa");

//SECCION DE ADMINISTRADORES

app.MapGet("/ListadoDeAdministrador", async (IRepoAdministrador _repo) => await _repo.ObtenerAsync).WithTags("Administradores");

app.MapGet("/Administrador/{id}", async (int id, IRepoAdministrador _repo) =>
{
    var resultado = await _repo.DetalleAsync(id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
}).WithTags("Administradores");

app.MapPost("/NuevoAdministrador", async (AdministradoresDTO nuevoadmin, IRepoAdministrador _repo) =>
{
    var fixture = new Administrador
    {
        IdAdministrador = 0,
        IdEmpresa = nuevoadmin.IdEmpresa,
        Nombre = nuevoadmin.Nombre,
        Password = nuevoadmin.Password
    };
    await _repo.AltaAsync(fixture);

    return Results.Created($"/Ruta/{fixture}", fixture);
}).WithTags("Administradores");

app.MapPut("/ActualizarAdministrador", async (AdministradoresDTO nuevoadmin, IRepoAdministrador _repo) =>
{ 
    var fixture = new Administrador
    {
        IdAdministrador = 0,
        Nombre = nuevoadmin.Nombre,
        Password = nuevoadmin.Password
    };
    await _repo.UpdateAdministrador(fixture);

    return Results.Created($"/ActualizarAdministrador/{fixture}", fixture);
}).WithTags("Administradores");

//CONDUCTORES

app.MapGet("/ListadoDeConductores", async (IRepoConductor _repo) => await _repo.ObtenerAsync).WithTags("Conductores");

app.MapGet("/Conductores/{id}", async (int id, IRepoConductor _repo) =>
{
    var resultado = await _repo.DetalleAsync(id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
}).WithTags("Conductores");


app.MapPost("/NuevoConductor", async (ConductoresDTO nuevoconductor, IRepoConductor _repo) =>
{
    var fixture = new Conductor
    {
        IdConductor = 0,
        Name = nuevoconductor.Name,
        Licencia = nuevoconductor.Licencia,
        Dispobilidad = true
    };
    await _repo.AltaAsync(fixture);

    return Results.Created($"/NuevoConductor/{fixture}", fixture);
}).WithTags("Conductores");

app.MapPatch("/ActualizarConductor", async (ConductorLicenciaDTO updateConductor, IRepoConductor repo) =>
{
    // Buscar el conductor actual
    var conductor = await repo.DetalleAsync(updateConductor.IdConductor);
    if (conductor == null)
        return Results.NotFound($"No se encontró el conductor con id {updateConductor.IdConductor}");

    conductor.Licencia = updateConductor.Licencia;

    await repo.UpdateConductorAsync(conductor);

    return Results.Ok(conductor);
}).WithTags("Conductores");

//Esto va ultimo, es la llave de arraque del ASP.NET.
await app.RunAsync();
