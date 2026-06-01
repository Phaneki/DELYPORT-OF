using Microsoft.AspNetCore.Mvc;

namespace Trabajo_Software.Controllers;

public class ClienteController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
