using System.Collections.Generic;
using Restaurante.Models;

namespace Restaurante.Models
{
    public class ReservasGestionViewModel
    {
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public string SortOrder { get; set; } = string.Empty;
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
