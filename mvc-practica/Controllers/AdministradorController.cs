using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using System.Collections;

namespace mvc_practica.Controllers;

public class AdministradorController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public IRepoEmpresa _repoEmpresa;
    public IRepoHisrorialPedido _repoHistorial;
    private IRepoAdministrador _repoAdmin;
    private IRepoVehiculo _repoVehiculo;
    private IRepoVehiculoConductor _repoVehCon;
    private IRepoPedido _repoPedido;
    private IRepoRuta _repoRuta;
    public AdministradorController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoHisrorialPedido repoHistorial, IRepoAdministrador repoAdministrador, IRepoVehiculo repoVehiculo, IRepoVehiculoConductor repoVehiculoConductor, IRepoPedido repoPedido, IRepoRuta repoRuta)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoHistorial = repoHistorial;
        _repoAdmin = repoAdministrador;
        _repoPedido = repoPedido;
        _repoRuta = repoRuta;
        _repoVehiculo = repoVehiculo;
    }

    public async Task<IActionResult> IndexAdmin(string nombre)
    {
        var administrador = await _repoAdmin.ObtenerCredenciales(nombre);
        HttpContext.Session.SetString("nombreAdministrador", nombre);
        HttpContext.Session.SetInt32("idAdministrador", administrador.IdAdministrador);
        var empresa = await _repoEmpresa.DetalleAsync((uint)administrador.IdEmpresa);
        var pedido = await _repoPedido.ObtenerPedidosEmpresa((int)empresa.IdEmpresa);
        HttpContext.Session.SetInt32("idEmpresa", (int)empresa.IdEmpresa);
        return View(pedido);
    }


    [HttpGet]
    public async Task<IActionResult> Historial()
    {
        var idEmpresa = HttpContext.Session.GetInt32("idEmpresa");

        var empresa = await _repoEmpresa.DetalleAsync((uint)idEmpresa);
        var _empresa = await _repoEmpresa.ObtenerPorNombreAsync(empresa.Nombre);
        var _HistorialPedido = await _repoHistorial.ObtenerHistorialCompleto((int)empresa.IdEmpresa);
        return View(_HistorialPedido);
    }

    [HttpPost]
    public async Task<IActionResult> AgregarRuta(Ruta _ruta)
    {
        if (ModelState.IsValid)
        {
            var _nuevaruta = new Ruta
            {
                Origen = _ruta.Origen,
                Destino = _ruta.Destino
            };

            await _repoRuta.AltaAsync(_nuevaruta);
            TempData["Mensaje"] = "Ruta Creada";
            var Nombre = HttpContext.Session.GetString("nombreAdministrador");
            return RedirectToAction("IndexAdmin", new { nombre = Nombre });
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AgregarPedido()
    {
        var idAdministrador = HttpContext.Session.GetInt32("idAdministrador");
        // Obtener datos necesarios
        var rutas = await _repoRuta.ObtenerAsync;
        var empresas = await _repoEmpresa.ObtenerAsync;
        var vehiculos = await _repoVehiculo.ObtenerVehiculosDisponibles();

        var dto = new PedidoViewModel
        {
            Titulo = string.Empty,
            Volumen = 0,
            Peso = 0,
            Estado = string.Empty,
            FechaDespacho = DateTime.Now,
            IdRuta = 0,
            IdEmpresaDestino = 0,
            IdVehiculo = 0,
            Rutas = rutas,
            Empresas = empresas,
            Vehiculos = vehiculos,
            IdAdministrador = (int)idAdministrador
        };

        return View(dto);
    }


    [HttpPost]
    public async Task<IActionResult> AgregarPedido(PedidoViewModel dto)
    {
        var idAdministrador = HttpContext.Session.GetInt32("idAdministrador");
        var pedido = new Pedido
        {
            NombrePedido = dto.Titulo,
            Volumen = dto.Volumen,
            Peso = dto.Peso,
            Estado = dto.Estado,
            FechaDespacho = DateTime.Today,
            XidRuta = dto.IdRuta,
            XidEmpresa = dto.IdEmpresaDestino,
            xidVehiculo = dto.IdVehiculo,
            XidAdministrador = (int)idAdministrador
        };

        await _repoPedido.AltaAsync(pedido);
        TempData["Mensaje"] = "Pedido creado exitosamente";

        var nombre = HttpContext.Session.GetString("nombreAdministrador");
        return RedirectToAction("IndexAdmin", new { nombre });
    }
    
    [HttpPost]
    public async Task<IActionResult> ActualizarEstado(int idPedido)
    {
        if (idPedido <= 0)
            return BadRequest("ID de pedido inválido.");

        await _repoPedido.ActualizarEstadoPedidoPorAdmin(idPedido, "Entregado");

        TempData["Mensaje"] = "Pedido marcado como entregado.";

        var nombre = HttpContext.Session.GetString("nombreAdministrador");
        return RedirectToAction("IndexAdmin", new { nombre });
    }
}