using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace mvc_practica.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
    public IActionResult Contact() => View();
    public IActionResult FAQ() => View();
    
    private readonly ILogger<HomeController> _logger;
    private readonly IRepoEmpresa _repoEmpresa;
    private readonly IRepoAdministrador _repoAdministrador;
    public Empresa _empresa;
    public Administrador _administrador;

    public HomeController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoAdministrador repoAdministrador)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoAdministrador = repoAdministrador;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    //Mth para loguearse y validar empresa
    [HttpPost]
    public async Task<IActionResult> Login_Empresa(IndexViewModel _empresa)
    {
        if (_empresa.Administrador == null)
        {
            var resultado = await _repoEmpresa.LoguearseAsync(_empresa.Empresa.Nombre.Trim().ToLower(), _empresa.Empresa.Contrasena);
            if (resultado == true)
            {
                return RedirectToAction("IndexEmpresa", "Empresa", new { nombre = _empresa.Empresa.Nombre });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        return View(_empresa);
    }

    //Mth para registrarse y crear empresa
    [HttpPost]
    public async Task<IActionResult> Post_Empresa(Empresa empresa)
    {
        if (ModelState.IsValid)
        {
            var _nuevaempresa = new Empresa
            {
                IdEmpresa = 0,
                Nombre = empresa.Nombre.Trim().ToLower(),
                Contrasena = empresa.Contrasena.Trim().ToLower()
            };

            await _repoEmpresa.AltaAsync(_nuevaempresa);

            TempData["Mensaje"] = "Empresa creada con éxito";
            return RedirectToAction("IndexEmpresa", "Empresa", new { nombre = empresa.Nombre }) ;
        }
        return View(empresa);
    }

    [HttpPost]
    public async Task<IActionResult> Login_Administrador(IndexViewModel _administrador)
    {
        if (_administrador.Empresa == null)
        {

            var repuesta = await _repoAdministrador.LoguearseAsync(_administrador.Administrador.Nombre, _administrador.Administrador.Password);
            if (repuesta == true)
                return RedirectToAction("indexAdmin", "Administrador", new { nombre = _administrador.Administrador.Nombre });
            else
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas. Por favor, inténtalo de nuevo.");
                // Si las credenciales no son válidas, muestra un mensaje de error o redirige a la página de inicio de sesión
            }
        }
        return View(_administrador);
    }
}

