using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;

namespace mvc_practica.Controllers;

public class ConductorController : Controller
{
    public IActionResult IndexConductor() => View();
    
    private readonly ILogger<HomeController> _logger;
    public ConductorController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

}