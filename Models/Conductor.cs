using System.ComponentModel.DataAnnotations;

namespace Trabajo_Software.Models;

public class Conductor
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Licencia { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Disponible"; // Estados: Disponible, Ocupado, Inactivo
}
