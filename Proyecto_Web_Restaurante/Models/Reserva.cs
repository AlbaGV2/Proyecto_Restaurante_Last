namespace Restaurante.Models;

public class Reserva
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int Personas { get; set; }
}
