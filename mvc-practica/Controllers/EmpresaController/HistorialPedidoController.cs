using Microsoft.AspNetCore.Mvc;
using mvc_practica.Controllers;

namespace EmpresaController;

public class HistorialPedidoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HistorialPedidoController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult HistorialPedido()
    {
        return View();
    }
}