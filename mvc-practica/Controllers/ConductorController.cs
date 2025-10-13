using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using System.Text;
using Microsoft.Identity.Client;

namespace mvc_practica.Controllers;

public class ConductorController : Controller
{
    private IRepoConductor _repoConductor;
    private IRepoVehiculoConductor _repoVehiculoConductor;
    private IRepoPedido _repoPedido;
    private IRepoVehiculo _repoVehiculo;
    private readonly ILogger<HomeController> _logger;
    public ConductorController(ILogger<HomeController> logger, IRepoConductor repoConductor, IRepoVehiculoConductor repoVehiculoConductor, IRepoPedido repoPedido, IRepoVehiculo repoVehiculo)
    {
        _logger = logger;
        _repoConductor = repoConductor;
        _repoVehiculoConductor = repoVehiculoConductor;
        _repoPedido = repoPedido;
        _repoVehiculo = repoVehiculo;
    }

    public async Task<IActionResult> IndexConductor(string nombre)
    {
        HttpContext.Session.SetString("Nombre", nombre);
        var _nombre = HttpContext.Session.GetString("Nombre");

        if (string.IsNullOrEmpty(_nombre))
            return RedirectToAction("Login", "Home");

        var conductor = await _repoConductor.ObtenerConductorPorNombre(_nombre);
        var _idvehiculoVinculado = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor);
        var pedidos = await _repoPedido.ObtenerPedidosPorVehiculo(_idvehiculoVinculado);
        return View(pedidos);
    }

    [HttpPost]
    public async Task<IActionResult> IniciarViaje()
    {
        var _nombre = HttpContext.Session.GetString("Nombre");
        if (string.IsNullOrEmpty(_nombre))
            return RedirectToAction("Login", "Home");

        var conductor = await _repoConductor.ObtenerConductorPorNombre(_nombre);
        await _repoConductor.ActualizarEstadoConductor(conductor.IdConductor); // En viaje

        await _repoVehiculo.ActualizarEstadoVehiculo(conductor.IdConductor); // En viaje
        await _repoPedido.ActualizarEstadoPedidoPorConductor(conductor.IdConductor, "En viaje");

        TempData["Mensaje"] = "Viaje iniciado correctamente.";
        return RedirectToAction("IndexConductor", _nombre);
    }

    [HttpPost]
    [Route("/Conductor/MarcarEntregado/{idPedido}")]
    public async Task<IActionResult> MarcarEntregado(int idPedido)
    {
        await _repoPedido.ActualizarEstadoPedidoPorConductor(idPedido, "Entregado");
        return Ok();
    }
}