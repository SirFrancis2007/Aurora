using Microsoft.AspNetCore.Mvc;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Eventing.Reader;
using Aurora.Core;
using Aurora.Core.Models;

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

    [Authorize(Roles = "conductor")] 
    public async Task<IActionResult> IndexConductor()
    {
        var nombre = User.Identity?.Name;

        if (string.IsNullOrEmpty(nombre))
            return RedirectToAction("Login", "Home");

        var conductor = await _repoConductor.ObtenerConductorPorNombre(nombre);
        var _idvehiculoVinculado = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor);
        var pedidos = await _repoPedido.ObtenerPedidosPorVehiculo(_idvehiculoVinculado);

        bool todosEntregados = pedidos.All(p => p.Estado == "Entregado");

        if (todosEntregados)
        {
            await _repoVehiculo.CambiarEstadoAsync(_idvehiculoVinculado, true);
            await _repoConductor.FncLiberarEstadoConductor(conductor.IdConductor);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }
        return View(pedidos);
    }

    [HttpPost]
    public async Task<IActionResult> IniciarViaje()
    {
        try
        {
            var nombre = User.Identity?.Name;
            if (string.IsNullOrEmpty(nombre))
                return RedirectToAction("Login", "Home");

            var conductor = await _repoConductor.ObtenerConductorPorNombre(nombre);
            var idVehiculo = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor);

            var pedidos = await _repoPedido.ObtenerPedidosPorVehiculo(idVehiculo);

            if (await FncVerificarPedidosEntregados(pedidos))
                return RedirectToAction("IndexConductor");

            if (await FncVerificarPedidosAlgunoEntregado(pedidos))
            {
                return RedirectToAction("IndexConductor");
            }    
            
            await _repoConductor.ActualizarEstadoConductor(conductor.IdConductor); // En viaje
            await _repoVehiculo.ActualizarEstadoVehiculo(idVehiculo);              // En viaje
            await _repoPedido.ActualizarEstadoPedidoPorConductor(conductor.IdConductor, "En viaje");

            return RedirectToAction("IndexConductor");
        }
        catch (System.Exception)
        {
            return RedirectToAction("IndexConductor");
            throw;
        }
    }

    internal async Task<bool> FncVerificarPedidosEntregados(IEnumerable<PedidoRutaDTO> pedidos)
    {
        if (pedidos.All(p => p.Estado == "Entregado"))
            return true;
        else
            return false;
    }

    internal async Task<bool> FncVerificarPedidosAlgunoEntregado(IEnumerable<PedidoRutaDTO> pedidos)
    {
        if (pedidos.Any(p => p.Estado == "Entregado"))
        {
            TempData["Mensaje"] = "Ya hay pedidos en viaje. No se puede reiniciar el viaje.";
            return true;
        }
        else
            return false;

    }

    [HttpPost]
    [Route("/Conductor/MarcarEntregado/{idPedido}")]
    public async Task<IActionResult> MarcarEntregado(int idPedido)
    {
        try
        {
            var nombre = User.Identity?.Name;
            var conductor = await _repoConductor.ObtenerConductorPorNombre(nombre);
            var idVehiculo = await _repoVehiculoConductor.ObtenerVehiculoPorIdConductor(conductor.IdConductor);

            // problema resuelto: El problema es que el mth hace una modificacion parcial, osea si se entrega uno, se entregan todos. Lo que genera incosistencia. Hayq ue crear un mth que solo modifique el estado del pedido individualmente. 
            await _repoPedido.ActualizarEstadoPedidoIndividual(idPedido, "Entregado");
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
        catch (System.Exception)
        {
            return BadRequest();
            throw;
        }
    }
}