using System.ComponentModel.DataAnnotations;

namespace Trabajo_Software.Models;

/// <summary>
/// Modelo que representa una solicitud de transporte
/// Incluye validaciones de Data Annotations para validación del lado servidor
/// </summary>
public class SolicitudTransporte
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Punto de partida del transporte
    /// </summary>
    [Required(ErrorMessage = "El campo Origen es obligatorio")]
    [StringLength(100, MinimumLength = 3, 
        ErrorMessage = "El Origen debe tener entre 3 y 100 caracteres")]
    [RegularExpression(@"^[a-zA-Z0-9\s\-,áéíóúñÁÉÍÓÚÑ]*$",
        ErrorMessage = "El Origen contiene caracteres no permitidos")]
    [Display(Name = "Origen", Description = "Ingrese la dirección de origen del transporte")]
    public string Origen { get; set; } = string.Empty;

    /// <summary>
    /// Distrito de destino para calcular la tarifa
    /// </summary>
    [Required(ErrorMessage = "El Distrito de Destino es obligatorio")]
    [Display(Name = "Distrito de Destino", Description = "Seleccione el distrito de destino")]
    public string DistritoDestino { get; set; } = string.Empty;

    /// <summary>
    /// Dirección exacta de llegada
    /// </summary>
    [Required(ErrorMessage = "La dirección de destino es obligatoria")]
    [StringLength(100, MinimumLength = 3, 
        ErrorMessage = "La dirección debe tener entre 3 y 100 caracteres")]
    [RegularExpression(@"^[a-zA-Z0-9\s\-,áéíóúñÁÉÍÓÚÑ.]*$",
        ErrorMessage = "La dirección contiene caracteres no permitidos")]
    [Display(Name = "Dirección Exacta (Destino)", Description = "Ingrese la dirección exacta de llegada")]
    public string Destino { get; set; } = string.Empty;

    /// <summary>
    /// Dimensiones del Producto
    /// </summary>
    [Required(ErrorMessage = "Debe seleccionar el tamaño del producto")]
    [Display(Name = "Dimensiones del Producto", Description = "Seleccione el tamaño de su carga")]
    public string DimensionesProducto { get; set; } = string.Empty;

    /// <summary>
    /// Precio Estimado
    /// </summary>
    [Display(Name = "Precio Estimado")]
    public decimal PrecioEstimado { get; set; }

    /// <summary>
    /// Fecha en la cual se realizará el servicio de transporte
    /// </summary>
    [Required(ErrorMessage = "La Fecha de servicio es obligatoria")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Servicio", Description = "Seleccione la fecha en que desea el servicio")]
    [FutureDate(ErrorMessage = "La fecha de servicio debe ser igual o posterior a la fecha actual")]
    public DateTime FechaServicio { get; set; }

    /// <summary>
    /// Hora en la cual se realizará el servicio de transporte
    /// </summary>
    [Required(ErrorMessage = "La Hora de servicio es obligatoria")]
    [DataType(DataType.Time)]
    [Display(Name = "Hora de Servicio", Description = "Seleccione la hora deseada para el servicio")]
    public TimeOnly HoraServicio { get; set; }

    /// <summary>
    /// Observaciones adicionales para el servicio
    /// </summary>
    [StringLength(500, MinimumLength = 5,
        ErrorMessage = "Las Observaciones deben tener entre 5 y 500 caracteres. Deje vacío si no desea incluir observaciones")]
    [RegularExpression(@"^[a-zA-Z0-9\s\-.,áéíóúñÁÉÍÓÚÑ]*$",
        ErrorMessage = "Las Observaciones contienen caracteres no permitidos")]
    [Display(Name = "Observaciones", Description = "Agregue detalles adicionales si es necesario")]
    public string? Observaciones { get; set; }

    /// <summary>
    /// Fecha de registro de la solicitud (se asigna automáticamente)
    /// </summary>
    [Display(Name = "Fecha de Solicitud")]
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;

    /// <summary>
    /// Estado actual de la solicitud
    /// </summary>
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "Pendiente";

    /// <summary>
    /// Identificador único para cada solicitud
    /// </summary>
    [Display(Name = "Identificador de Solicitud")]
    public string IdentificadorUnico { get; set; } = string.Empty;

    /// <summary>
    /// Observaciones agregadas por el operador durante la validación
    /// </summary>
    [Display(Name = "Observaciones de Validación")]
    [StringLength(500)]
    public string? ObservacionesValidacion { get; set; }

    /// <summary>
    /// ID del Conductor asignado
    /// </summary>
    public int? ConductorId { get; set; }

    public Conductor? Conductor { get; set; }

    /// <summary>
    /// Fecha en la que se asignó un conductor
    /// </summary>
    [Display(Name = "Fecha de Asignación")]
    public DateTime? FechaAsignacion { get; set; }
}

/// <summary>
/// Validador personalizado para validar que la fecha sea futura o actual
/// Se utiliza en la propiedad FechaServicio
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FutureDateAttribute : ValidationAttribute
{
    /// <summary>
    /// Valida que la fecha sea igual o posterior a la fecha actual
    /// </summary>
    /// <param name="value">Valor a validar</param>
    /// <returns>true si la fecha es válida, false si no lo es</returns>
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is not DateTime dateValue)
            return false;

        // Comparar solo la fecha, sin la hora
        return dateValue.Date >= DateTime.Now.Date;
    }
}

