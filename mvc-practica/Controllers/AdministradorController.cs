using Microsoft.AspNetCore.Mvc;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

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

    [Authorize(Roles = "administrador")]
    public async Task<IActionResult> IndexAdmin()
    {
        var nombre = User.Identity?.Name;

        var administrador = await _repoAdmin.ObtenerCredenciales(nombre);
        if (administrador == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        var empresa = await _repoEmpresa.DetalleAsync((uint)administrador.IdEmpresa);
        var pedidos = await _repoPedido.ObtenerPedidosEmpresa((int)empresa.IdEmpresa);

        ViewBag.NombreAdministrador = nombre;
        ViewBag.IdEmpresa = empresa.IdEmpresa;

        return View(pedidos);
    }


    [HttpGet]
    public async Task<IActionResult> Historial()
    {
        try
        {
            var nombre = User.Identity?.Name;
            var administrador = await _repoAdmin.ObtenerCredenciales(nombre);
            var empresa = await _repoEmpresa.DetalleAsync(administrador.IdEmpresa);
            var _empresa = await _repoEmpresa.ObtenerPorNombreAsync(empresa.Nombre);
            var _HistorialPedido = await _repoHistorial.ObtenerHistorialCompleto((int)empresa.IdEmpresa);
            return View(_HistorialPedido);
        }
        catch (System.Exception)
        {
            return View(0);
            throw;
        }
        
    }

    public IActionResult AgregarRuta() => View();
    [HttpPost]
    public async Task<IActionResult> AgregarRuta(Ruta _ruta)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var _nuevaRuta = new Ruta
                {
                    Origen = LimpiarCampo(_ruta.Origen, nameof(_ruta.Origen)),
                    Destino = LimpiarCampo(_ruta.Destino, nameof(_ruta.Destino))
                };
                await _repoRuta.AltaAsync(_nuevaRuta);
                TempData["Mensaje"] = "Ruta Creada";
                return RedirectToAction("IndexAdmin");
            }
            return View();
        }
        catch (System.Exception)
        {
            return View();
            throw;
        }
        
    }

    private string LimpiarCampo(string valor, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentNullException(nombreCampo, $"El campo {nombreCampo} no puede estar vacío.");

        return valor.Trim().ToLower();
    }

    [HttpGet]
    public async Task<IActionResult> AgregarPedido()
    {
        var nombre = User.Identity?.Name;
        var administrador = await _repoAdmin.ObtenerCredenciales(nombre);

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
            IdAdministrador = administrador.IdAdministrador
        };

        return View(dto);
    }


    [HttpPost]
    //Este mth es literalmetne la accion de alta pedido
    public async Task<IActionResult> AgregarPedido(PedidoViewModel dto)
    {
        try
        {
            var nombre = User.Identity?.Name;
            var administrador = await _repoAdmin.ObtenerCredenciales(nombre);
            var pedido = new Pedido
            {
                NombrePedido = dto.Titulo.Trim().ToLower(),
                Volumen = await VefNumeros(dto.Volumen),
                Peso = await VefNumeros(dto.Peso),
                Estado = dto.Estado,
                FechaDespacho = DateTime.Today,
                XidRuta = dto.IdRuta,
                XidEmpresa = dto.IdEmpresaDestino,
                xidVehiculo = dto.IdVehiculo,
                XidAdministrador = administrador.IdAdministrador
            };

            await _repoPedido.AltaAsync(pedido);        
            TempData["Mensaje"] = "Pedido creado exitosamente";

            return RedirectToAction("IndexAdmin");
        }
        catch (System.Exception)
        {
            return View(IndexAdmin());
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> MarcarComoEntregado(int idPedido)
    {
        try
        {
            if (idPedido <= 0) return BadRequest("ID de pedido inválido.");
            await _repoPedido.ActualizarEstadoPedidoPorAdmin(idPedido, "Recibido");
            TempData["Mensaje"] = "Pedido marcado como Recibido.";
            return RedirectToAction("IndexAdmin");
        }
        catch (System.Exception)
        {
            return View(IndexAdmin());
            throw;
        }
        
    }
    
    private async Task<double> VefNumeros(double n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "El campo no acepta medidas negativas.");

        return await Task.FromResult(n);    
    }
}