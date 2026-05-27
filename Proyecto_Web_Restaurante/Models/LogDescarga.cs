using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models
{
    public class LogDescarga
    {
        [Key]
        public int Id { get; set; }

        // Propiedad calculada para el ID estilo D0001
        [NotMapped]
        public string IdFormateado => $"D{Id:D4}";

        public int? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }

        public DateTime FechaDescarga { get; set; } = DateTime.Now;
        
        [Required]
        public string? Formato { get; set; } // PDF, Excel, CSV

        public DateTime? FiltroFechaInicio { get; set; }
        public DateTime? FiltroFechaFin { get; set; }
        
        public int TotalRegistros { get; set; }
        public string? IpAddress { get; set; }
    }
}
