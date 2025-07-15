using System.ComponentModel.DataAnnotations;
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

app.MapGet("/Ruta/{id}", async (int id, IRepoRuta _repo) =>
{
    var resultado = await _repo.DetalleAsync(id);
    return resultado is not null
        ? Results.Ok(resultado)
        : Results.NotFound();
});


// METODO POST DE RUTAS

app.MapPost("/Ruta", async (Ruta todo, IRepoRuta _repo) =>
{
    await _repo.AltaAsync(todo);

    return Results.Created($"/Ruta/{todo.IdRuta}", todo);
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

public struct EmpresaDTO
{
    [Required]
    public string Nombre {get; set;}
}

public struct EmpresaPedidoDTO
{
    public EmpresaPedidoDTO()
    {
    }

    public uint IdEmpresa {get; set;}
    [Required]
    public string Nombre {get; set;}
    public List<PedidoDTO> Pedidos { get; set; } = new();
}

public struct PedidoDTO {
    public int IdPedido { get; set; }
    public int XidAdministrador { get; set; }
    public int XidRuta { get; set; }
    public int XidEmpresa { get; set; }
    public int xidVehiculo { get; set;}
    public required string NombrePedido { get; set; }
    public required double Peso { get; set; }
    public required double Volumen { get; set; }
    public required string Estado { get; set; }
    public required DateTime FechaDespacho { get; set; }
}