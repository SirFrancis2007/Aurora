using Aurora.Core;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace mvc_practica.Controllers;

public class EmpresaController : Controller
{
    private string _nombreempresa;
    public IActionResult HistorialPedido() => View();
    public IActionResult VehiculoEmpresa() => View();
    public IActionResult ConductorEmpresa() => View();
    public IActionResult AgregarAdministrador() => View("UIAdministrador/AgregarAdministrador");
    public IActionResult NuevoConductor() => View("UIConductor/NuevoConductor");
    public IActionResult AgregarVehiculo() => View("UIVehiculo/AgregarVehiculo");
    public IActionResult AsignarVehiculoAConductor() => View("UIVehiculo/AsignarVehiculoAConductor");
    //Cuando una vista se encuentra en una carpeta dentro de Views, se debe especificar la ruta completa
    //Ejemplo: return View("UIAdministrador/AgregarAdministrador");
    private readonly ILogger<HomeController> _logger;
    private IRepoEmpresa _repoEmpresa;
    private IRepoAdministrador _repoAdmin;
    public EmpresaController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
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
}