using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using System.Collections;

namespace mvc_practica.Controllers;

public class AdministradorController : Controller
{
    public IActionResult AgregarPedido() => View();
    public IActionResult AgregarRuta() => View();
    private readonly ILogger<HomeController> _logger;
    public IRepoEmpresa _repoEmpresa;
    public IRepoHisrorialPedido _repoHistorial;
    private IRepoAdministrador _repoAdmin;
    private IRepoVehiculo _repoVehiculo;
    private IRepoVehiculoConductor _repoVehCon;
    private IRepoPedido _repoPedido;
    public AdministradorController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoHisrorialPedido repoHistorial, IRepoAdministrador repoAdministrador, IRepoVehiculo repoVehiculo, IRepoVehiculoConductor repoVehiculoConductor)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoHistorial = repoHistorial;
        _repoAdmin = repoAdministrador;
    }

    public async Task<IActionResult> IndexAdmin(string nombre, string contrasena)
    {
        HttpContext.Session.SetString("DatosAdministrador", nombre);

        var administrador = await _repoAdmin.ObtenerCredenciales(nombre);
        var empresa = await _repoEmpresa.DetalleAsync(administrador.IdEmpresa);
        var datosempresa = await _repoEmpresa.ObtenerPorNombreAsync(empresa.Nombre);
        var _pedidos = await _repoPedido.ObtenerPedidosEmpresa((int)datosempresa.IdEmpresa);
        return View(_pedidos);
    }


    [HttpGet]
    public async Task<IActionResult> Historial()
    {
        // se debera traer el id empresa del administrador loguado para despues trer el nombre de la empresa al que pertenece y finalmente traer el historial.


        var nombre = HttpContext.Session.GetString("NombreEmpresa");

        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre);
        var _HistorialPedido = await _repoHistorial.ObtenerHistorialCompleto((int)empresa.IdEmpresa);

        return View(_HistorialPedido);
    }
}