using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

/// <summary>
/// Representa un evento tecnológico administrado por la API.
/// </summary>
public class Evento : IValidatableObject
{
    /// <summary>
    /// Identificador único del evento.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nombre del evento (obligatorio).
    /// </summary>
    [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Categoría del evento (ej. Hackatón, Conferencia, Taller).
    /// </summary>
    [Required(ErrorMessage = "La categoría del evento es obligatoria.")]
    [StringLength(80, ErrorMessage = "La categoría no puede exceder los 80 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora programada del evento.
    /// </summary>
    [Required(ErrorMessage = "La fecha del evento es obligatoria.")]
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Lugar o sede donde se realizará el evento.
    /// </summary>
    [Required(ErrorMessage = "El lugar del evento es obligatorio.")]
    [StringLength(200, ErrorMessage = "El lugar no puede exceder los 200 caracteres.")]
    public string Lugar { get; set; } = string.Empty;

    /// <summary>
    /// Cupo máximo de asistentes (debe ser mayor que cero).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor que cero.")]
    public int CupoMaximo { get; set; }

    /// <summary>
    /// Indica si el evento se encuentra activo.
    /// </summary>
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Validaciones adicionales de negocio para el modelo.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            yield return new ValidationResult(
                "El nombre del evento es obligatorio.",
                new[] { nameof(Nombre) });
        }

        if (CupoMaximo <= 0)
        {
            yield return new ValidationResult(
                "El cupo máximo debe ser mayor que cero.",
                new[] { nameof(CupoMaximo) });
        }

        if (Fecha == default || Fecha.Year < 2000 || Fecha.Year > 2100)
        {
            yield return new ValidationResult(
                "La fecha del evento debe ser válida (entre los años 2000 y 2100).",
                new[] { nameof(Fecha) });
        }
    }
}
