using System.ComponentModel.DataAnnotations;

namespace Restaurante.Models;

public class Reserva
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre no puede superar los 30 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    [StringLength(50, ErrorMessage = "El email no puede superar los 50 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Fecha { get; set; }

    [Range(1, 20, ErrorMessage = "El número de personas debe estar entre 1 y 20.")]
    public int Personas { get; set; }

    // Turno seleccionado por el usuario: "Comida" o "Cena"
    [Required(ErrorMessage = "El turno es obligatorio.")]
    public string Turno { get; set; } = string.Empty;

    // Hora seleccionada por el usuario, ej: "14:00", "21:30"
    [Required(ErrorMessage = "La hora es obligatoria.")]
    public string Hora { get; set; } = string.Empty;
}
