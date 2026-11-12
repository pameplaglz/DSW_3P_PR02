using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Data;

/// <summary>
/// Contexto de Entity Framework Core para la base de datos de eventos tecnológicos.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Evento> Eventos => Set<Evento>();
}
