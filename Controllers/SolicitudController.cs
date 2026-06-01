using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabajo_Software.Data;
using Trabajo_Software.Models;

namespace Trabajo_Software.Controllers;

/// <summary>
/// Controlador para gestionar las solicitudes de transporte
/// Maneja las operaciones CRUD de solicitudes de transporte
/// </summary>
public class SolicitudController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<SolicitudController> _logger;

    public SolicitudController(AppDbContext context, ILogger<SolicitudController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Acción GET: Muestra la lista de solicitudes de transporte
    /// </summary>
    /// <returns>Vista con lista de solicitudes</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var solicitudes = await _context.SolicitudesTransporte
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();

            return View(solicitudes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las solicitudes de transporte");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// Acción GET: Muestra la lista de solicitudes pendientes de validación
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PendientesValidacion()
    {
        try
        {
            var solicitudesRegistradas = await _context.SolicitudesTransporte
                .Where(s => s.Estado == "Pendiente")
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();

            return View(solicitudesRegistradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las solicitudes pendientes de validación");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// Acción GET: Muestra la lista de solicitudes validadas listas para asignación de conductor
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Validadas()
    {
        try
        {
            var solicitudesValidadas = await _context.SolicitudesTransporte
                .Where(s => s.Estado == "Validada")
                .OrderBy(s => s.FechaServicio).ThenBy(s => s.HoraServicio)
                .ToListAsync();

            return View(solicitudesValidadas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las solicitudes validadas");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Aprobar(int id, string? observaciones)
    {
        var solicitud = await _context.SolicitudesTransporte.FindAsync(id);
        if (solicitud != null)
        {
            solicitud.Estado = "Validada";
            solicitud.ObservacionesValidacion = observaciones;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(PendientesValidacion));
    }

    [HttpPost]
    public async Task<IActionResult> Rechazar(int id, string? observaciones)
    {
        var solicitud = await _context.SolicitudesTransporte.FindAsync(id);
        if (solicitud != null)
        {
            solicitud.Estado = "Rechazada";
            solicitud.ObservacionesValidacion = observaciones;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(PendientesValidacion));
    }

    /// <summary>
    /// Acción GET: Muestra el formulario para crear una nueva solicitud
    /// </summary>
    /// <returns>Vista del formulario</returns>
    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    /// <summary>
    /// Acción POST: Procesa el formulario y guarda la solicitud en la base de datos
    /// Realiza todas las validaciones del lado servidor antes de guardar
    /// </summary>
    /// <param name="solicitud">Objeto SolicitudTransporte con los datos del formulario</param>
    /// <returns>Redirige a la vista de confirmación o vuelve al formulario si hay errores</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(SolicitudTransporte solicitud)
    {
        // Paso 1: Validar que ModelState sea válido (valida todas las Data Annotations)
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Validación de ModelState fallida. Errores: {ErrorCount}", 
                ViewData.ModelState.ErrorCount);
            
            // Mostrar los errores en la consola para debugging
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogWarning("Error de validación: {ErrorMessage}", error.ErrorMessage);
                }
            }
            
            return View(solicitud);
        }

        // Paso 2: Validaciones adicionales del lado servidor (más allá de Data Annotations)
        
        // Validar que Origen y Destino sean diferentes
        if (solicitud.Origen.Equals(solicitud.Destino, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("Destino", 
                "El Destino no puede ser igual al Origen. Por favor, ingrese una dirección diferente.");
            _logger.LogWarning("Validación fallida: Origen y Destino son iguales");
            return View(solicitud);
        }

        // Validar que la hora sea válida si la fecha es hoy
        if (solicitud.FechaServicio == DateTime.Today)
        {
            var horaActual = TimeOnly.FromDateTime(DateTime.Now);
            if (solicitud.HoraServicio <= horaActual)
            {
                ModelState.AddModelError("HoraServicio", 
                    $"La Hora de servicio debe ser posterior a la hora actual ({horaActual:HH:mm})");
                _logger.LogWarning("Validación fallida: Hora posterior a la actual cuando la fecha es hoy");
                return View(solicitud);
            }
        }

        // Paso 3: Validar que no exista una solicitud duplicada en la misma fecha y hora
        var solicitudDuplicada = await _context.SolicitudesTransporte
            .AnyAsync(s => s.FechaServicio == solicitud.FechaServicio &&
                          s.HoraServicio == solicitud.HoraServicio &&
                          s.Origen.ToLower() == solicitud.Origen.ToLower() &&
                          s.Destino.ToLower() == solicitud.Destino.ToLower() &&
                          s.Estado != "Cancelada");

        if (solicitudDuplicada)
        {
            ModelState.AddModelError("FechaServicio", 
                "Ya existe una solicitud de transporte con los mismos datos (origen, destino, fecha y hora). " +
                "Por favor, modifique los datos de la solicitud.");
            _logger.LogWarning("Validación fallida: Solicitud duplicada detectada");
            return View(solicitud);
        }

        try
        {
            // Paso 4: Calcular Precio Estimado en el servidor para evitar manipulación
            decimal precioBase = 0;
            if (solicitud.DistritoDestino == "Lima Centro") precioBase = 40;
            else if (solicitud.DistritoDestino == "Lima Moderna") precioBase = 50;
            else if (solicitud.DistritoDestino == "Lima Periferica") precioBase = 70;

            decimal recargo = 0;
            if (solicitud.DimensionesProducto == "Pequeño") recargo = 0;
            else if (solicitud.DimensionesProducto == "Mediano") recargo = 20;
            else if (solicitud.DimensionesProducto == "Grande") recargo = 50;

            solicitud.PrecioEstimado = precioBase + recargo;

            // Paso 5: Asignar valores de auditoría
            solicitud.FechaSolicitud = DateTime.Now;
            solicitud.Estado = "Pendiente";
            solicitud.IdentificadorUnico = Guid.NewGuid().ToString("N").ToUpper();

            // Limpiar observaciones vacías
            if (string.IsNullOrWhiteSpace(solicitud.Observaciones))
            {
                solicitud.Observaciones = null;
            }
            else
            {
                // Trimear espacios en blanco
                solicitud.Observaciones = solicitud.Observaciones.Trim();
            }

            // Paso 5: Guardar en la base de datos
            _context.SolicitudesTransporte.Add(solicitud);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Solicitud de transporte creada exitosamente. ID: {SolicitudId}, Origen: {Origen}, Destino: {Destino}, Fecha: {Fecha}",
                solicitud.Id, solicitud.Origen, solicitud.Destino, solicitud.FechaServicio);

            // Paso 6: Redirigir a la acción de confirmación
            return RedirectToAction(nameof(Confirmacion), new { id = solicitud.Id });
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Error de base de datos al guardar la solicitud");
            ModelState.AddModelError("", 
                "Error en la base de datos: No se pudo guardar la solicitud. Por favor, intente de nuevo más tarde.");
            return View(solicitud);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al guardar la solicitud");
            ModelState.AddModelError("", 
                "Ocurrió un error inesperado: No se pudo procesar la solicitud. Por favor, intente de nuevo.");
            return View(solicitud);
        }
    }

    /// <summary>
    /// Acción GET: Muestra la página de confirmación de la solicitud registrada
    /// </summary>
    /// <param name="id">ID de la solicitud creada</param>
    /// <returns>Vista de confirmación con los datos de la solicitud</returns>
    [HttpGet]
    public async Task<IActionResult> Confirmacion(int id)
    {
        try
        {
            var solicitud = await _context.SolicitudesTransporte.FindAsync(id);

            if (solicitud == null)
            {
                _logger.LogWarning($"Solicitud no encontrada. ID: {id}");
                return NotFound();
            }

            return View(solicitud);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la confirmación de la solicitud");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// Acción GET: Muestra los detalles de una solicitud específica
    /// </summary>
    /// <param name="id">ID de la solicitud</param>
    /// <returns>Vista con los detalles de la solicitud</returns>
    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        try
        {
            var solicitud = await _context.SolicitudesTransporte.FindAsync(id);

            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la solicitud");
            return RedirectToAction("Error", "Home");
        }
    }
}
