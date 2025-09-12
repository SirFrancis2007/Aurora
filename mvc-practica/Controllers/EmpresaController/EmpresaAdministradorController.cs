using Microsoft.AspNetCore.Mvc;
using mvc_practica.Controllers;

namespace EmpresaController;

public class EmpresaAdministradorController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public EmpresaAdministradorController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult EmpresaAdministrador()
    {
        return View();
    }
}