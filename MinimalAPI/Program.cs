using System.Data;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
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

app.MapGet("/Ruta/{id}", (int id, IRepoRuta _repo) =>
    _repo.DetalleAsync(id)
        is IRepoRuta todo
            ? Results.Ok(todo)
            : Results.NotFound());

// METODO POST DE RUTAS

app.MapPost("/Ruta", async (Ruta todo, IRepoRuta _repo) =>
{
    await _repo.AltaAsync(todo);

    return Results.Created($"/Ruta/{todo.IdRuta}", todo);
});

// ------------------- ENTIDAD EMPRESA ---------------------------------- //

//METODO GET PARA ENTIDAD EMPRESA
app.MapGet("/Empresa", async (IRepoEmpresa _repoempresa) 
    => _repoempresa.ObtenerAsync);

app.MapGet("/Empresa/{id}", async (uint id, IRepoEmpresa _repoempresa) =>
    await _repoempresa.DetalleAsync(id)
        is IRepoEmpresa empresa
            ? Results.Ok(empresa)
            : Results.NotFound());

//METODO PARA ELIMINAR ENTIDAD EMPRESA
app.MapDelete("/Empresa/{id}", async (uint id, IRepoEmpresa _repoempresa) =>
{
    if (await _repoempresa.DetalleAsync(id) is IRepoEmpresa todo)
    {
        await _repoempresa.EliminarAdministradorAsync((int)id);
        await _repoempresa.EliminarEmpresaAsync((int)id);
        return Results.NoContent();
    }

    return Results.NotFound();
});

// METODO PARA CREAR EMPRESA
app.MapPost("/Empresa",  async (Empresa empresa, IRepoEmpresa _repoempresa) =>
{
    await _repoempresa.AltaAsync(empresa);
    return Results.Created($"/Empresa/{empresa.IdEmpresa}", empresa);
});


//Esto va ultimo, es la llave de arraque del ASP.NET.
await app.RunAsync();
