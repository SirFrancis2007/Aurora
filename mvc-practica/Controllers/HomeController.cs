using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using System.Xml.Schema;

namespace mvc_practica.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
    public IActionResult Contact() => View();
    public IActionResult FAQ() => View();
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
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
    public IActionResult Registro(Empresa empresa)
    {
        if (ModelState.IsValid)
        {
            //Contactar con ADO para crear la empresa
            return RedirectToAction("Index");
        }

        return View(empresa);
    }
}

