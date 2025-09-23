using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mvc_practica.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        public IActionResult Forbidden() => View("Error/Forbidden");
            public IActionResult GenericError() => View("Error/GenericError");
            public IActionResult NotFound() => View("Error/NotFound");
            public IActionResult ServerError() => View("Error/ServerError");

        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/StatusCode")]
        public IActionResult StatusCodeHandler(int code)
        {
            switch (code)
            {
                case 404:
                    return View("NotFound");
                case 403:
                    return View("Forbidden");
                default:
                    return View("GenericError");
            }
        }

        [Route("Error/ServerError")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}