using System.Collections.Generic;
using Restaurante.Models;

namespace Restaurante.Models
{
    public class ReservasGestionViewModel
    {
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
        public List<Reserva> ReservasEliminadas { get; set; } = new List<Reserva>();
        public List<LogDescarga> LogDescargas { get; set; } = new List<LogDescarga>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public string SortOrder { get; set; } = string.Empty;
        public int TotalReservas { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? SearchString { get; set; }
        public Dictionary<DateTime, int> ConteosPorFecha { get; set; } = new Dictionary<DateTime, int>();
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
