using Trabajo_Software.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Trabajo_Software.Models.ViewModels;

public class AsignarViewModel
{
    public SolicitudTransporte Solicitud { get; set; } = null!;
    public int ConductorId { get; set; }
    public IEnumerable<SelectListItem> ConductoresDisponibles { get; set; } = new List<SelectListItem>();
}
