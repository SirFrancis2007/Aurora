using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace mvc_practica.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult login() => View();
    
    private readonly ILogger<HomeController> _logger;
    private readonly IRepoEmpresa _repoEmpresa;
    private readonly IRepoAdministrador _repoAdministrador;
    private readonly IRepoConductor _repoConductor;
    public Empresa _empresa;
    public Administrador _administrador;
    public Conductor _conductor;

    public HomeController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoAdministrador repoAdministrador, IRepoConductor repoConductor)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoAdministrador = repoAdministrador;
        _repoConductor = repoConductor;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    //Mth para registrarse y crear empresa
    [HttpPost]
    public async Task<IActionResult> Post_Empresa(AutenticacionDTO _datos)
    {
        if (ModelState.IsValid)
        {
            var _nuevaempresa = new Empresa
            {
                IdEmpresa = 0,
                Nombre = _datos.Usuario.Trim().ToLower(),
                Contrasena = _datos.Contrasena.Trim().ToLower()
            };

            await _repoEmpresa.AltaAsync(_nuevaempresa);

            TempData["Mensaje"] = "Empresa creada con éxito";
            return RedirectToAction("IndexEmpresa", "Empresa", new { nombre = _datos.Usuario });
        }
        return View(_datos);
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginUniversal(AutenticacionDTO _datos)
    {
        bool resultado = false;

        var _vefCredenciales = new AutenticacionDTO
        {
            Usuario = _datos.Usuario?.Trim().ToLower(),
            Contrasena = _datos.Contrasena?.Trim().ToLower(),
            Rol = _datos.Rol?.Trim().ToLower()
        };

        switch (_vefCredenciales.Rol.ToLower())
        {
            case "empresa":
                resultado = await _repoEmpresa.LoguearseAsync(_vefCredenciales.Usuario, _vefCredenciales.Contrasena);
                if (resultado)
                    return RedirectToAction("IndexEmpresa", "Empresa", new { nombre = _vefCredenciales.Usuario });
                break;

            case "admin":
                resultado = await _repoAdministrador.LoguearseAsync(_vefCredenciales.Usuario, _vefCredenciales.Contrasena);
                if (resultado)
                    return RedirectToAction("IndexAdmin", "Administrador", new { nombre = _vefCredenciales.Usuario });
                break;

            case "conductor":
                resultado = await _repoConductor.Loguearse(_vefCredenciales.Usuario, _vefCredenciales.Contrasena);
                if (resultado)
                    return RedirectToAction("IndexConductor", "Conductor", new { nombre = _vefCredenciales.Usuario });
                break;

            default:
                ModelState.AddModelError("", "Rol no válido.");
                break;
        }

        ModelState.AddModelError("", "Credenciales incorrectas");
        return View("Index");
    }
}

