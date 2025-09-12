using System.Diagnostics;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Aurora.Dapper.ADO;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Controllers;
using mvc_practica.Models;

namespace EmpresaController;

public class EmpresaController : Controller
{
    public IActionResult EmpresaAdministrador() => View();
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
    public Empresa _empresa;
    private IRepoEmpresa _repoEmpresa;
    public EmpresaController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
    }
    public IActionResult IndexEmpresa()
    {
        return View();
    }

    //Mth para ver los pedidos de la empresa
    public async Task<IActionResult> VerPedidos(Empresa _empresa)
    {
        var pedidos = await _repoEmpresa.ObtenerPedidosAsync((int)_empresa.IdEmpresa);
        return View();
    }
}