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

    [HttpPost]
    public async Task<IActionResult> AutenticacionConductor(Conductor _conductor)
    {
        try
        {
            var repuesta = await _repoConductor.Loguearse(_conductor.Name, _conductor.Licencia);
            if (repuesta == true)
            {
                HttpContext.Session.SetString("Nombre", _conductor.Name);
                return RedirectToAction(nameof(ConductorController.IndexConductor));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas. Por favor, inténtalo de nuevo.");
                // Si las credenciales no son válidas, muestra un mensaje de error o redirige a la página de inicio de sesión
            }
        }
        catch
        {
            return NotFound();
        }
        return View();
    }

    public async Task<IActionResult> IndexConductor()
    {
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
        var nombre = HttpContext.Session.GetString("nombreConductor");
        if (string.IsNullOrEmpty(nombre))
            return RedirectToAction("Login", "Home");

        var conductor = await _repoConductor.ObtenerConductorPorNombre(nombre);
        await _repoConductor.ActualizarEstadoConductor(conductor.IdConductor); // En viaje

        await _repoVehiculo.ActualizarEstadoVehiculo(conductor.IdConductor); // En viaje
        await _repoPedido.ActualizarEstadoPedidoPorConductor(conductor.IdConductor, "En viaje");

        TempData["Mensaje"] = "Viaje iniciado correctamente.";
        return RedirectToAction("IndexConductor");
    }

    [HttpPost]
    [Route("Conductor/MarcarEntregado/{idPedido}")]
    public async Task<IActionResult> MarcarEntregado(int idPedido)
    {
        await _repoPedido.ActualizarEstadoPedidoPorConductor(idPedido, "Entregado");
        return Ok();
    }
}