using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

/// <summary>
/// Implementación del repositorio de eventos con persistencia en SQL Server mediante Entity Framework Core.
/// </summary>
public class EfEventoRepository : IEventoRepository
{
    private readonly AppDbContext _context;

    public EfEventoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Evento>> GetAllAsync()
    {
        return await _context.Eventos.AsNoTracking().ToListAsync();
    }

    public async Task<Evento?> GetByIdAsync(int id)
    {
        return await _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Evento> AddAsync(Evento evento)
    {
        evento.Id = 0; // Permitir que SQL Server genere el IDENTITY
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task<Evento?> UpdateAsync(Evento evento)
    {
        var existing = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == evento.Id);
        if (existing is null)
        {
            return null;
        }

        existing.Nombre = evento.Nombre;
        existing.Categoria = evento.Categoria;
        existing.Fecha = evento.Fecha;
        existing.Lugar = evento.Lugar;
        existing.CupoMaximo = evento.CupoMaximo;
        existing.Activo = evento.Activo;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (existing is null)
        {
            return false;
        }

        _context.Eventos.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
