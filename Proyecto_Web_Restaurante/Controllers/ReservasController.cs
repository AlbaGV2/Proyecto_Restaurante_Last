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
        public async Task<IActionResult> Gestion(string sortOrder, string searchString, int pagina = 1, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            int registrosPorPagina = 10;
            
            ViewData["CurrentFilter"] = searchString;
            
            // Parámetros para las cabeceras (toggle entre asc y desc)
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["PersonSortParm"] = sortOrder == "Personas" ? "person_desc" : "Personas";

            var query = _context.Reservas.Where(r => !r.IsDeleted).AsQueryable();

            // Filtrado por rango de fechas
            if (fechaInicio.HasValue)
            {
                query = query.Where(r => r.Fecha >= fechaInicio.Value.Date);
            }
            if (fechaFin.HasValue)
            {
                query = query.Where(r => r.Fecha <= fechaFin.Value.Date);
            }

            // Búsqueda por nombre o ID de reserva
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(r => r.Nombre.Contains(searchString) || r.IdReserva.Contains(searchString));
            }

            // Total de reservas filtradas
            var totalCount = await query.CountAsync();
            
            // Reservas eliminadas (Papelera)
            var eliminadas = await _context.Reservas
                .Where(r => r.IsDeleted)
                .OrderByDescending(r => r.FechaEliminacion)
                .Take(10)
                .ToListAsync();

            // Lógica de ordenación
            query = sortOrder switch
            {
                "id_desc" => query.OrderByDescending(r => r.IdReserva),
                "id_asc" => query.OrderBy(r => r.IdReserva),
                "nombre_desc" => query.OrderByDescending(r => r.Nombre),
                "nombre_asc" => query.OrderBy(r => r.Nombre),
                "fecha_asc" => query.OrderBy(r => r.Fecha).ThenBy(r => r.Hora),
                _ => query.OrderByDescending(r => r.Fecha).ThenByDescending(r => r.Hora), // Default
            };
            
            int totalPaginas = (int)Math.Ceiling((double)totalCount / registrosPorPagina);
            
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
                SortOrder = sortOrder,
                TotalReservas = totalCount,
                ReservasEliminadas = eliminadas,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                SearchString = searchString,
                ConteosPorFecha = await _context.Reservas
                    .Where(r => !r.IsDeleted)
                    .GroupBy(r => r.Fecha.Date)
                    .Select(g => new { Fecha = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Fecha, x => x.Count),
                LogDescargas = await _context.LogDescargas
                    .OrderByDescending(l => l.FechaDescarga)
                    .Take(10)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarDescarga([FromBody] LogDescarga log)
        {
            if (log == null) return BadRequest();

            log.FechaDescarga = DateTime.Now;
            log.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";
            
            // Los campos UsuarioId y UsuarioNombre vendrán NULL de momento como pidió el usuario
            
            _context.LogDescargas.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, idFormateado = log.IdFormateado });
        }

        [HttpGet]
        public async Task<IActionResult> GetHorasOcupadas(DateTime fecha)
        {
            var horasOcupadas = await _context.Reservas
                .Where(r => r.Fecha.Date == fecha.Date && !r.IsDeleted)
                .Select(r => r.Hora)
                .ToListAsync();

            return Ok(horasOcupadas);
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
            if (reserva.Fecha < DateTime.Now.Date)
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

            // 3. Verificación de negocio: No permitir reservas los lunes (Cerrado)
            if (reserva.Fecha.DayOfWeek == DayOfWeek.Monday)
            {
                return BadRequest(new
                {
                    success = false,
                    errores = new
                    {
                        Fecha = new string[] { "El restaurante permanece cerrado los lunes. Por favor, elija otro día." }
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
            reserva.FechaCreacion = DateTime.Now;

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
        // GET: /Reservas/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            return View(reserva);
        }

        // POST: /Reservas/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Email,Fecha,Personas,Turno,Hora,IdReserva")] Reserva reserva)
        {
            if (id != reserva.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    reserva.UltimaModificacion = DateTime.Now;
                    reserva.WasRestored = false; // Si se edita, ya no se marca solo como restaurada
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Reserva actualizada correctamente.";
                    return RedirectToAction(nameof(Gestion));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id)) return NotFound();
                    else throw;
                }
            }
            return View(reserva);
        }

        // POST: /Reservas/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            reserva.IsDeleted = true;
            reserva.FechaEliminacion = DateTime.Now;
            
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Reserva eliminada correctamente.";
            return RedirectToAction(nameof(Gestion));
        }

        // POST: /Reservas/Restaurar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            reserva.IsDeleted = false;
            reserva.FechaEliminacion = null;
            reserva.WasRestored = true;
            reserva.UltimaModificacion = DateTime.Now;
            
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Reserva restaurada correctamente.";
            return RedirectToAction(nameof(Gestion));
        }

        // POST: /Reservas/EliminarDefinitivamente/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDefinitivamente(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Reserva eliminada definitivamente.";
            return RedirectToAction(nameof(Gestion));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }

        // GET: /Reservas/GetReservasJSON
        [HttpGet]
        public async Task<IActionResult> GetReservasJSON(DateTime? inicio, DateTime? fin)
        {
            var query = _context.Reservas.Where(r => !r.IsDeleted).AsQueryable();

            if (inicio.HasValue)
                query = query.Where(r => r.Fecha >= inicio.Value.Date);
            
            if (fin.HasValue)
                query = query.Where(r => r.Fecha <= fin.Value.Date);

            var reservas = await query
                .OrderByDescending(r => r.Fecha)
                .ThenByDescending(r => r.Hora)
                .Select(r => new {
                    r.IdReserva,
                    Fecha = r.Fecha.ToString("dd/MM/yyyy"),
                    r.Hora,
                    r.Nombre,
                    r.Email,
                    r.Personas,
                    r.Turno
                })
                .ToListAsync();

            return Ok(reservas);
        }
    }
}
