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
        await _repoConductor.ActualizarEstadoConductor(conductor.IdConductor); // En viaje //esta aca esta bien
        var idvehiculo = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor); //  Aca se obtiene el id del vehiculo a partir de una consulta hecha al la tabla vehiculo-conductor (del muhco a muchos) en base al id del conductor.
        // hay que actualizar el estado del vehiculo en base al conductor asignado, asi que hay que ver vehiculo-conductor y ahi actualizar el estado del vehiculo para que se actualice el vehiculo asginado al conductor
        await _repoVehiculo.ActualizarEstadoVehiculo(idvehiculo);  // En viaje

        /*var pedidos = await _repoPedido.ObtenerPedidosPorVehiculo(idvehiculo);
        var existePedidoEntregasos = pedidos.Any(p => p.Estado == "Entregado");
        if (pedidos.Count > 0)
        {
            return RedirectToAction("Login", "Home");
        }
        else
        {
            // Pedido tiene el idVehiculo, asi que hay que obtener el idVehiculo del conductor y ahi actualizar el estado del pedido
            await _repoPedido.ActualizarEstadoPedidoPorConductor(conductor.IdConductor, "En viaje");
        }*/

        await _repoPedido.ActualizarEstadoPedidoPorConductor(conductor.IdConductor, "En viaje");
        
        TempData["Mensaje"] = "Viaje iniciado correctamente.";
        return RedirectToAction("IndexConductor", new { nombre = _nombre });
    }

    [HttpPost]
    [Route("/Conductor/MarcarEntregado/{idPedido}")]
    public async Task<IActionResult> MarcarEntregado(int idPedido)
    {
        await _repoPedido.ActualizarEstadoPedidoPorConductor(idPedido, "Entregado");
        return Ok();
    }
}