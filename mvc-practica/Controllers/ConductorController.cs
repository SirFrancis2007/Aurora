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

        bool todosEntregados = pedidos.All(p => p.Estado == "Entregado");

        if (todosEntregados)
        {
            await _repoVehiculo.CambiarEstadoAsync(_idvehiculoVinculado, true);
            await _repoConductor.FncLiberarEstadoConductor(conductor.IdConductor);
            return RedirectToAction("Login", "Home");
        }
        return View(pedidos);
    }

    [HttpPost]
    public async Task<IActionResult> IniciarViaje()
    {
        var _nombre = HttpContext.Session.GetString("Nombre");
        if (string.IsNullOrEmpty(_nombre))
            return RedirectToAction("Login", "Home");

        var conductor = await _repoConductor.ObtenerConductorPorNombre(_nombre);
        var idVehiculo = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor);

        var pedidos = await _repoPedido.ObtenerPedidosPorVehiculo(idVehiculo);

        if (pedidos.All(p => p.Estado == "Entregado"))
        {
            TempData["Mensaje"] = "Todos los pedidos ya fueron entregados. No se puede iniciar un nuevo viaje.";
            return RedirectToAction("IndexConductor", new { nombre = _nombre });
        }

        if (pedidos.Any(p => p.Estado == "Entregado"))
        {
            TempData["Mensaje"] = "Ya hay pedidos en viaje. No se puede reiniciar el viaje.";
            return RedirectToAction("IndexConductor", new { nombre = _nombre });
        }

        await _repoConductor.ActualizarEstadoConductor(conductor.IdConductor); // En viaje
        await _repoVehiculo.ActualizarEstadoVehiculo(idVehiculo);              // En viaje
        await _repoPedido.ActualizarEstadoPedidoPorConductor(conductor.IdConductor, "En viaje");

        TempData["Mensaje"] = "Viaje iniciado correctamente.";
        return RedirectToAction("IndexConductor", new { nombre = _nombre });
    }

    [HttpPost]
    [Route("/Conductor/MarcarEntregado/{idPedido}")]
    public async Task<IActionResult> MarcarEntregado(int idPedido)
    {
        var _nombre = HttpContext.Session.GetString("Nombre");
        var conductor = await _repoConductor.ObtenerConductorPorNombre(_nombre);
        var idVehiculo = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor);

        // El problema es que el mth hace una modificacion parcial, osea si se entrega uno, se entregan todos. Lo que genera incosistencia. Hayq ue crear un mth que solo modifique el estado del pedido individualmente. 
        await _repoPedido.ActualizarEstadoPedidoPorConductor(idPedido, "Entregado");
        await _repoVehiculo.RestaurarPesoVehiculo(idVehiculo, idPedido);

        var pedidos = await _repoPedido.ObtenerPedidosPorVehiculo(idVehiculo);
        bool todosEntregados = pedidos.All(p => p.Estado == "Entregado");

        if (todosEntregados)
        {
            await _repoVehiculo.CambiarEstadoAsync(idVehiculo, true);
            await _repoConductor.FncLiberarEstadoConductor(conductor.IdConductor);
        }

        return Ok();
    }

}