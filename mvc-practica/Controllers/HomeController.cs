using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc_practica.Models;
using Aurora.Core;
using Aurora.Core.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Aurora.Dapper.ADO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace mvc_practica.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult login() => View();
    
    private readonly ILogger<HomeController> _logger;
    private readonly IRepoEmpresa _repoEmpresa;
    private readonly IRepoAdministrador _repoAdministrador;
    private readonly IRepoConductor _repoConductor;
    public Empresa _empresa;
    public Administrador _administrador;
    public Conductor _conductor;
    public readonly IRepoAutenticacion _repoAutenticacion;
    public readonly RepoAutenticar _repoAutenticar;

    public HomeController(ILogger<HomeController> logger, IRepoEmpresa repoEmpresa, IRepoAdministrador repoAdministrador, IRepoConductor repoConductor, IRepoAutenticacion repoAutenticacion, RepoAutenticar repoAutenticar)
    {
        _logger = logger;
        _repoEmpresa = repoEmpresa;
        _repoAdministrador = repoAdministrador;
        _repoConductor = repoConductor;
        _repoAutenticacion = repoAutenticacion;
        _repoAutenticar = repoAutenticar;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    //Mth para registrarse y crear empresa
    [HttpPost]
    public async Task<IActionResult> Post_Empresa(AutenticacionDTO _datos)
    {
        if (ModelState.IsValid)
        {
            var _nuevaempresa = new Empresa
            {
                IdEmpresa = 0,
                Nombre = _datos.Usuario.Trim().ToLower(),
                Contrasena = _datos.Contrasena.Trim().ToLower()
            };

            await _repoEmpresa.AltaAsync(_nuevaempresa);

            TempData["Mensaje"] = "Empresa creada con éxito";
            return RedirectToAction("IndexEmpresa", "Empresa", new { nombre = _datos.Usuario });
        }
        return View(_datos);
    }

    [HttpPost]
    public async Task<IActionResult> LoginUniversal(AutenticacionDTO datos)
    {
        var usuario = datos.Usuario?.Trim().ToLower();
        var contrasena = datos.Contrasena?.Trim().ToLower();
        var rol = datos.Rol?.Trim().ToLower();

        var repo = _repoAutenticar.ObtenerPorRol(rol);

        if (repo == null)
        {
            ModelState.AddModelError("", "Rol no válido.");
            return View("Index");
        }

        bool autenticado = await repo.LoguearseAsync(usuario, contrasena);

        if (!autenticado)
        {
            ModelState.AddModelError("", "Credenciales incorrectas.");
            return View("Index");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, rol)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddHours(2)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        return RedirectToAction(
            repo.ObtenerAccionRedireccion(),
            repo.ObtenerControladorRedireccion(),
            new { nombre = usuario });
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Home");
    }
}

