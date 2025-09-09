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
    private Empresa _empresa;

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
    public IActionResult Index(Empresa empresa)
    {
        if (ModelState.IsValid)
        {
            //Contactar con ADO para validar la empresa
            return RedirectToAction("About");
        }

        return View(empresa);
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
}

