using Aurora.Core;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using mvc_practica.Models;
using Xunit.Sdk;

namespace mvc_practica.Controllers;

public class EmpresaController : Controller
{
    //Cuando una vista se encuentra en una carpeta dentro de Views, se debe especificar la ruta completa
    //Ejemplo: return View("UIAdministrador/AgregarAdministrador");
    private string _nombreempresa;
    public IActionResult AgregarAdministrador() => View("UIAdministrador/AgregarAdministrador");
    public IActionResult NuevoConductor() => View("UIConductor/NuevoConductor");
    public IActionResult AgregarVehiculo() => View("UIVehiculo/AgregarVehiculo");
    private readonly ILogger<HomeController> _logger;
    private IRepoEmpresa _repoEmpresa;
    private IRepoAdministrador _repoAdmin;
    private IRepoVehiculo _repoVehiculo;
    private IRepoConductor _repoConductor;
    private IRepoVehiculoConductor _repoVehCon;
    private IRepoPedido _repoPedido;
    private IRepoHisrorialPedido _repoHisrorialPedido;
    public EmpresaController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoAdministrador repoAdmin, IRepoVehiculo repoVehiculo, IRepoConductor repoConductor, IRepoVehiculoConductor repoVehiculoConductor, IRepoPedido repoPedido, IRepoHisrorialPedido repoHisrorialPedido) // <-- agrega los Int de c/u repo
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoAdmin = repoAdmin;
        _repoVehiculo = repoVehiculo;
        _repoConductor = repoConductor;
        _repoVehCon = repoVehiculoConductor;
        _repoPedido = repoPedido;
        _repoHisrorialPedido = repoHisrorialPedido;
    }

    [HttpGet]
    public async Task<IActionResult> IndexEmpresa(string nombre)
    {
        HttpContext.Session.SetString("NombreEmpresa", nombre);

        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre);
        var pedidos = await _repoPedido.ObtenerPedidosPorEmpresa((int)empresa.IdEmpresa);

        return View(pedidos);
    }

    [HttpGet]
    public async Task<IActionResult> EmpresaAdministrador()
    {
        var nombre = HttpContext.Session.GetString("NombreEmpresa");

        var empresa = await _repoEmpresa.ObtenerPorNombreAsync(nombre);
        var administradores = await _repoAdmin.ObtenerPorEmpresaAsync((int)empresa.IdEmpresa);

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
                Nombre = _admin.Nombre.Trim(),
                Password = _admin.Password.Trim(),
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
                Tipo = _vehiculo.Tipo.Trim(),
                Matricula = _vehiculo.Matricula.Trim(),
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
                Name = _conductor.Name.Trim(),
                Licencia = _conductor.Licencia.Trim(),
                Disponibilidad = true //Disponible por defecto
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
        var _HistorialPedido = await _repoHisrorialPedido.ObtenerHistorialCompleto((int)empresa.IdEmpresa);

        return View(_HistorialPedido);
    }

    [HttpGet]
    public async Task<IActionResult> AsignarVehiculoAConductor()
    {
        var viewModel = new AsignacionVehiculoViewModel
        {
            ConductoresAsignados = await _repoVehCon.ConsultaConductoresAsignados(),
            ConductoresNoAsignados = await _repoVehCon.ConsultaConductoresNOAsignados(),
            VehiculosNoAsignados = await _repoVehCon.consultaVehiculoLibres()
        };

        return View("UIVehiculo/AsignarVehiculoAConductor", viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AltaAsignacionVehiculoConductor(int idConductor, int idVehiculo, DateOnly FAsignacion)
    {
        // inst de obj veh + con
        var _nuevaasignacion = new VehiculoConductor
        {
            XidConductor = idConductor,
            XidVehiculo = idVehiculo,
            FechaAsignacion = DateTime.Now
        };

        await _repoVehCon.AltaAsync(_nuevaasignacion);

        TempData["Mensaje"] = "La asignacion fue exitosa";
        return RedirectToAction(nameof(EmpresaController.AsignarVehiculoAConductor));
    }

    [HttpPost]
    public async Task<IActionResult> EliminarAdministrador(int idAdministrador)
    {
        await _repoAdmin.EliminarAsync(idAdministrador);
        return RedirectToAction(nameof(EmpresaAdministrador));
    }

    public async Task<IActionResult> ActualizarConductor(int id)
    {
        var conductor = await _repoConductor.DetalleAsync(id);
        if (conductor == null)
            return NotFound();

        HttpContext.Session.SetInt32("IdConductorActualizar", id);
        return View("UIConductor/ActualizarConductor", conductor);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateConductor(Conductor _conductor)
    {
        var idConductor = HttpContext.Session.GetInt32("IdConductorActualizar");
        while (await VefDisponibilidadConductor((int)idConductor))
        {
            if (!ModelState.IsValid)
                return View("UIConductor/ActualizarConductor", _conductor);

            var conductor = new Conductor
            {
                IdConductor = (int)idConductor,
                Name = _conductor.Name.Trim(),
                Licencia = _conductor.Licencia.Trim(),
                Disponibilidad = _conductor.Disponibilidad = true
            };

            await _repoConductor.ActualizarConductor(conductor);
            return RedirectToAction(nameof(ConductorEmpresa));
        }
        return RedirectToAction(nameof(ConductorEmpresa));
    }

    private async Task<bool> VefDisponibilidadConductor(int id)
    {
        Conductor resultado = await _repoConductor.DetalleAsync(id);
        if (resultado.Disponibilidad == false)
            return false;
        else
            return true;
    }
    
    [HttpPost]
    public async Task<IActionResult> DesvincularVehiculoConductor(int idConductor, int idVehiculo)
    {
        try
        {
            await _repoVehCon.DesasignarConductorDeVehiculo(idConductor, idVehiculo);
            await _repoVehiculo.CambiarEstadoAsync(idVehiculo, true);
            await _repoConductor.FncLiberarEstadoConductor(idConductor);
            TempData["Mensaje"] = "Vehículo desvinculado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrió un error al desvincular el vehículo: " + ex.Message;
        }

        return RedirectToAction("AsignarVehiculoAConductor"); 
    }
}