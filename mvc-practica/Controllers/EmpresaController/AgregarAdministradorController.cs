using Microsoft.AspNetCore.Mvc;
using mvc_practica.Controllers;

namespace EmpresaController;

public class AgregarAdministradorController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AgregarAdministradorController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult AgregarAdministrador()
    {
        return View();
    }
}