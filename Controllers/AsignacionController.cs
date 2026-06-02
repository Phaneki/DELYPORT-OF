using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabajo_Software.Data;

namespace Trabajo_Software.Controllers;

public class AsignacionController : Controller
{
    private readonly AppDbContext _context;

    public AsignacionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ConductoresDisponibles()
    {
        var conductores = await _context.Conductores
            .ToListAsync();

        return View(conductores);
    }

    [HttpGet]
    public async Task<IActionResult> Asignar(int id)
    {
        var solicitud = await _context.SolicitudesTransporte.FindAsync(id);
        if (solicitud == null || solicitud.Estado != "Validada")
        {
            return NotFound();
        }

        var conductores = await _context.Conductores
            .Where(c => c.Estado == "Disponible")
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.NombreCompleto} - Licencia: {c.Licencia}"
            })
            .ToListAsync();

        var viewModel = new Trabajo_Software.Models.ViewModels.AsignarViewModel
        {
            Solicitud = solicitud,
            ConductoresDisponibles = conductores
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AsignarConductor(int solicitudId, int conductorId)
    {
        var solicitud = await _context.SolicitudesTransporte.FindAsync(solicitudId);
        var conductor = await _context.Conductores.FindAsync(conductorId);

        if (solicitud != null && conductor != null && solicitud.Estado == "Validada" && conductor.Estado == "Disponible")
        {
            solicitud.ConductorId = conductorId;
            solicitud.Estado = "Asignado";
            solicitud.FechaAsignacion = DateTime.Now;
            conductor.Estado = "Ocupado";

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(ConductoresDisponibles)); // O redireccionar a un listado de asignadas
    }
}
