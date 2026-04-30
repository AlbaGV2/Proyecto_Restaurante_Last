using Microsoft.AspNetCore.Mvc;
using Restaurante.Data;
using Restaurante.Models;

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

        // POST: /Reservas/Crear
        // Recibe los datos del formulario en formato JSON desde el frontend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear([FromBody] Reserva reserva)
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

            
            // TODO: cuando la base de datos esté disponible, descomentar estas líneas:
            // _context.Reservas.Add(reserva);
            // await _context.SaveChangesAsync();

            // Por ahora devolvemos una confirmación sin guardar en BD
            return Ok(new
            {
                success = true,
                mensaje = $"Reserva recibida correctamente para {reserva.Nombre} el {reserva.Fecha:dd/MM/yyyy} a las {reserva.Hora} ({reserva.Turno}) para {reserva.Personas} persona(s)."
            });
        }
    }
}
