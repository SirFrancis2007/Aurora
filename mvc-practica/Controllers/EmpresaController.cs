using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;

namespace EmpresaController;

public class EmpresaController : Controller
{
    public IActionResult IndexEmpresa()
    {
        return View();
    }
}