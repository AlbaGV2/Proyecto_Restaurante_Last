using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Restaurante.Data;
using Restaurante.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Restaurante.Controllers
{
    public class ReservasController : Controller
    {
        private readonly RestauranteContext _context;

        public ReservasController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: /Reservas
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Reservas/Gestion
        public async Task<IActionResult> Gestion(string sortOrder, int pagina = 1)
        {
            int registrosPorPagina = 10;
            
            // Parámetros para las cabeceras (toggle entre asc y desc)
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["PersonSortParm"] = sortOrder == "Personas" ? "person_desc" : "Personas";

            var query = _context.Reservas.AsQueryable();

            // Lógica de ordenación
            query = sortOrder switch
            {
                "date_asc" => query.OrderBy(r => r.Fecha).ThenBy(r => r.Hora),
                "Name" => query.OrderBy(r => r.Nombre),
                "name_desc" => query.OrderByDescending(r => r.Nombre),
                "Id" => query.OrderBy(r => r.IdReserva),
                "id_desc" => query.OrderByDescending(r => r.IdReserva),
                "Personas" => query.OrderBy(r => r.Personas),
                "person_desc" => query.OrderByDescending(r => r.Personas),
                _ => query.OrderByDescending(r => r.Fecha).ThenByDescending(r => r.Hora), // Default
            };
            
            int totalRegistros = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            
            pagina = pagina < 1 ? 1 : pagina;
            if (totalPaginas > 0 && pagina > totalPaginas) pagina = totalPaginas;

            var reservas = await query
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            var viewModel = new ReservasGestionViewModel
            {
                Reservas = reservas,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        // POST: /Reservas/Crear
        // Recibe los datos del formulario en formato JSON desde el frontend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([FromBody] Reserva reserva)
        {
            // Validamos el modelo con las reglas definidas en Reserva.cs
            if (!ModelState.IsValid)
            {
                // Devolvemos los errores de validación al frontend
                var errores = ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errores });
            }

            // 2. Verificación de negocio: No permitir reservas con fecha pasada
            if (reserva.Fecha < DateTime.Now)
            {
                return BadRequest(new
                {
                    success = false,
                    errores = new
                    {
                        Fecha = new string[] { "No se pueden realizar reservas para fechas pasadas." }
                    }
                });
            }

            // 3. Verificación de negocio: Limitar capacidad máxima (ej: 20 personas)
            if (reserva.Personas > 20)
            {
                return BadRequest(new
                {
                    success = false,
                    errores = new
                    {
                        Personas = new string[] { "El número de personas excede la capacidad máxima de la reserva." }
                    }
                });
            }

            // 4. Verificación de negocio: Validar turnos/horarios
            var hora = TimeSpan.Parse(reserva.Hora);
            var turno = reserva.Turno.ToLower();
            
            // Validación para "comida" (mediodía) - 12:00 a 16:00
            if (turno == "comida" && (hora < new TimeSpan(12, 0, 0) || hora > new TimeSpan(16, 0, 0)))
            {
                return BadRequest(new
                {
                    success = false,
                    errores = new
                    {
                        Hora = new string[] { "El horario de comida es de 12:00 a 16:00." }
                    }
                });
            }

            // Validación para "cena" (noche) - 19:00 a 23:00
            if (turno == "cena" && (hora < new TimeSpan(19, 0, 0) || hora > new TimeSpan(23, 0, 0)))
            {
                return BadRequest(new
                {
                    success = false,
                    errores = new
                    {
                        Hora = new string[] { "El horario de cena es de 19:00 a 23:00." }
                    }
                });
            }

            // 5. Verificación de negocio: No permitir reservas con menos de 1 hora de anticipación
            var fechaHoraReserva = reserva.Fecha.Date + TimeSpan.Parse(reserva.Hora);
            if (fechaHoraReserva - DateTime.Now < TimeSpan.FromHours(1))
            {
                return BadRequest(new
                {
                    success = false,
                    errores = new
                    {
                        FechaHora = new string[] { "La reserva debe realizarse con al menos 1 hora de anticipación." }
                    }
                });
            }

            // 6. Generar un código de reserva único (IdReserva)
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string codigoAleatorio = new string(Enumerable.Repeat(caracteres, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            
            reserva.IdReserva = $"RES-{codigoAleatorio}";

            try
            {
                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    mensaje = $"¡Reserva confirmada! Su código es {reserva.IdReserva}. Le esperamos el {reserva.Fecha:dd/MM/yyyy} a las {reserva.Hora}."
                });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, new { success = false, mensaje = "Error al guardar en la base de datos. Verifique la conexión con SQL Server." });
            }
        }
    }
}
