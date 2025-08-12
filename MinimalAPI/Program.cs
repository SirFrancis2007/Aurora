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

app.MapGet("/Ruta", async (IRepoRuta _repo) => await _repo.ObtenerAsync);

app.MapGet("/Ruta/{id}", async (int id, IRepoRuta _repo) =>
{
    var resultado = await _repo.DetalleAsync(id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
});


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
});

// ------------------- ENTIDAD EMPRESA ---------------------------------- //

//METODO GET PARA ENTIDAD EMPRESA
app.MapGet("/Empresa", async (IRepoEmpresa _repoempresa) =>
{
    var empresas = await _repoempresa.ObtenerAsync;
    return Results.Ok(empresas);
});


app.MapGet("/Empresa/{id}", async (uint id, IRepoEmpresa _repoempresa) =>
{
    var resultado = await _repoempresa.ObtenerPedidosAsync((int)id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
});

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
});


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
});

//Esto va ultimo, es la llave de arraque del ASP.NET.
await app.RunAsync();

