using Microsoft.EntityFrameworkCore;
using Restaurante.Models;

namespace Restaurante.Data;

public class RestauranteContext : DbContext
{
    // Constructor necesario por Entity Framework
    public RestauranteContext(DbContextOptions<RestauranteContext> options) : base(options)
    {
    }

    // Esta propiedad le dice a SQL que cree una tabla llamada "Reservas"
    public DbSet<Reserva> Reservas { get; set; }
}
