using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

/// <summary>
/// Servicio de aplicación que aplica las reglas de negocio para eventos tecnológicos.
/// </summary>
public class EventoService : IEventoService
{
    private readonly IEventoRepository _repository;

    public EventoService(IEventoRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Evento>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Evento?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<Evento> CreateAsync(Evento evento)
    {
        ValidarEvento(evento);
        return _repository.AddAsync(evento);
    }

    public Task<Evento?> UpdateAsync(int id, Evento evento)
    {
        ValidarEvento(evento);
        evento.Id = id;
        return _repository.UpdateAsync(evento);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static void ValidarEvento(Evento evento)
    {
        if (string.IsNullOrWhiteSpace(evento.Nombre))
        {
            throw new ArgumentException("El nombre del evento es obligatorio.", nameof(evento.Nombre));
        }

        if (evento.CupoMaximo <= 0)
        {
            throw new ArgumentException("El cupo máximo debe ser mayor que cero.", nameof(evento.CupoMaximo));
        }

        if (evento.Fecha == default || evento.Fecha.Year < 2000 || evento.Fecha.Year > 2100)
        {
            throw new ArgumentException("La fecha del evento debe ser válida (entre los años 2000 y 2100).", nameof(evento.Fecha));
        }
    }
}
