using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using System.Xml.Schema;
using Aurora.Dapper.ADO;
using AspNetCoreGeneratedDocument;

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

    public HomeController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
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
            await _repoEmpresa.LoginAsync(_empresa.Empresa.Nombre);
            return RedirectToAction("IndexEmpresa", "Empresa");
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
                Nombre = empresa.Nombre
            };

            await _repoEmpresa.AltaAsync(_nuevaempresa);

            TempData["Mensaje"] = "Empresa creada con éxito";
            return RedirectToAction("IndexEmpresa", "Empresa");
        }
        return View(empresa);
    }

    [HttpPost]
    public async Task<IActionResult> Login_Administrador(Administrador administrador)
    {
        if (ModelState.IsValid)
        {
            var _nuevoAdmin = new Administrador
            {
                Nombre = administrador.Nombre,
                Password = administrador.Password
            };
            return RedirectToAction("IndexAdmin", "Admin");
        }

        // Si las credenciales no son válidas, muestra un mensaje de error o redirige a la página de inicio de sesión
        ModelState.AddModelError(string.Empty, "Credenciales inválidas. Por favor, inténtalo de nuevo.");
        return View(administrador);
    }
}

