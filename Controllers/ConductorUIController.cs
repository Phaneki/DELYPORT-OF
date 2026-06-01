using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabajo_Software.Data;

namespace Trabajo_Software.Controllers;

public class ConductorUIController : Controller
{
    private readonly AppDbContext _context;

    public ConductorUIController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string licencia)
    {
        if (string.IsNullOrWhiteSpace(licencia))
        {
            ModelState.AddModelError("", "La licencia es requerida.");
            return View("Index");
        }

        var conductor = await _context.Conductores.FirstOrDefaultAsync(c => c.Licencia == licencia);
        
        if (conductor == null)
        {
            ModelState.AddModelError("", "Licencia no encontrada en el sistema.");
            return View("Index");
        }

        return RedirectToAction(nameof(MisViajes), new { conductorId = conductor.Id });
    }

    [HttpGet]
    public async Task<IActionResult> MisViajes(int conductorId)
    {
        var conductor = await _context.Conductores.FindAsync(conductorId);
        if (conductor == null) return NotFound();

        ViewBag.ConductorNombre = conductor.NombreCompleto;
        ViewBag.ConductorId = conductor.Id;

        var viajes = await _context.SolicitudesTransporte
            .Where(s => s.ConductorId == conductorId && s.Estado != "Completado")
            .OrderBy(s => s.FechaServicio)
            .ThenBy(s => s.HoraServicio)
            .ToListAsync();

        return View(viajes);
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarViaje(int solicitudId)
    {
        var solicitud = await _context.SolicitudesTransporte.FindAsync(solicitudId);
        if (solicitud != null && solicitud.Estado == "Asignado")
        {
            solicitud.Estado = "Completado";
            var conductor = await _context.Conductores.FindAsync(solicitud.ConductorId);
            if (conductor != null)
            {
                conductor.Estado = "Disponible"; // El conductor vuelve a estar disponible
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MisViajes), new { conductorId = solicitud.ConductorId });
        }
        return BadRequest();
    }
}
