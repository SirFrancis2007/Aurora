using Aurora.Core;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Xunit.Sdk;

namespace mvc_practica.Controllers;

public class EmpresaController : Controller
{
    private string _nombreempresa;
    public IActionResult AgregarAdministrador() => View("UIAdministrador/AgregarAdministrador");
    public IActionResult NuevoConductor() => View("UIConductor/NuevoConductor");
    public IActionResult AgregarVehiculo() => View("UIVehiculo/AgregarVehiculo");
    //Cuando una vista se encuentra en una carpeta dentro de Views, se debe especificar la ruta completa
    //Ejemplo: return View("UIAdministrador/AgregarAdministrador");
    private readonly ILogger<HomeController> _logger;
    private IRepoEmpresa _repoEmpresa;
    private IRepoAdministrador _repoAdmin;
    private IRepoVehiculo _repoVehiculo;
    private IRepoConductor _repoConductor;
    public EmpresaController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoAdministrador repoAdmin, IRepoVehiculo repoVehiculo, IRepoConductor repoConductor) // <-- agrega este parámetro
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoAdmin = repoAdmin;
        _repoVehiculo = repoVehiculo;
        _repoConductor = repoConductor;
    }

    [HttpGet]
    public async Task<IActionResult> IndexEmpresa(string nombre)
    {
        HttpContext.Session.SetString("NombreEmpresa", nombre);

        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre);
        var pedidos = await _repoEmpresa.ObtenerPedidosAsync((int)empresa.IdEmpresa);

        return View(pedidos);
    }

    [HttpGet]
    public async Task<IActionResult> EmpresaAdministrador()
    {
        var nombre = HttpContext.Session.GetString("NombreEmpresa");

        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre);
        var administradores = await _repoEmpresa.ObtenerAdministradoresXempresaAsync((int)empresa.IdEmpresa);

        return View(administradores);
    }

    [HttpGet]
    public async Task<IActionResult> VehiculoEmpresa()
    {
        var vehiculos = await _repoVehiculo.ObtenerAsync;
        return View(vehiculos);
    }

    [HttpGet]
    public async Task<IActionResult> ConductorEmpresa()
    {
        var conductores = await _repoConductor.ObtenerAsync;
        return View(conductores);
    }

    [HttpPost]
    public async Task<IActionResult> AltaAdministrador(Administrador _admin)
    {
        var nombre = HttpContext.Session.GetString("NombreEmpresa");
        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre); //agarrar el id de la empresa logueada  


        if (ModelState.IsValid)
        {
            var _nuevoadmin = new Administrador
            {
                IdAdministrador = 0,
                Nombre = _admin.Nombre,
                Password = _admin.Password,
                IdEmpresa = empresa.IdEmpresa
            };

            await _repoAdmin.AltaAsync(_nuevoadmin);

            TempData["Mensaje"] = "Administrador creada con éxito";
            return RedirectToAction(nameof(EmpresaController.EmpresaAdministrador));
        }
        return View(empresa);
    }

    [HttpPost]
    public async Task<IActionResult> AltaVehiculo(Vehiculo _vehiculo)
    {
        if (ModelState.IsValid)
        {
            var _nuevovehiculo = new Vehiculo
            {
                IdVehiculo = 0,
                Tipo = _vehiculo.Tipo,
                Matricula = _vehiculo.Matricula,
                CapacidadMax = _vehiculo.CapacidadMax,
                Estado = true //Disponible por defecto
            };

            await _repoVehiculo.AltaAsync(_nuevovehiculo);

            TempData["Mensaje"] = "Administrador creada con éxito";
            return RedirectToAction(nameof(EmpresaController.VehiculoEmpresa));
        }
        return View(_vehiculo);
    }

    [HttpPost]
    public async Task<IActionResult> AltaConductor(Conductor _conductor)
    {
        if (ModelState.IsValid)
        {
            var _nuevoconductor = new Conductor
            {
                IdConductor = 0,
                Name = _conductor.Name,
                Licencia = _conductor.Licencia,
                Dispobilidad = true //Disponible por defecto
            };

            await _repoConductor.AltaAsync(_nuevoconductor);

            TempData["Mensaje"] = "Conductor creado con éxito";
            return RedirectToAction(nameof(EmpresaController.ConductorEmpresa));
        }
        return View(_conductor);
    }

    [HttpGet]
    public async Task<IActionResult> HistorialPedido()
    {
        var nombre = HttpContext.Session.GetString("NombreEmpresa");

        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre);
        var _HistorialPedido = await _repoEmpresa.ObtenerHistorialXempresaAsync((int)empresa.IdEmpresa);

        return View(_HistorialPedido);
    }

    [HttpGet]
    public async Task<IActionResult> AsignarVehiculoAConductor()
    {
        var conductores = await _repoConductor.ListarConductoresSinVehiculoAsync();
        var vehiculos = await _repoVehiculo.ListarVehiculosSinConductorAsync();

        var viewModel = new AsignarVehiculoViewModel
        {
            Conductores = conductores,
            Vehiculos = vehiculos
        };

        return View("UIVehiculo/AsignarVehiculoAConductor", viewModel);
    }
}