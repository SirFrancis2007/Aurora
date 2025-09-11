using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace mvc_practica.Controllers;

public class AdministradorController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public AdministradorController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult IndexAdmin()
    {
        return View();
    }
}