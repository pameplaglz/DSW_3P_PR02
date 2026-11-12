using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Services;

/// <summary>
/// Contrato de lógica de negocio para la gestión de eventos tecnológicos.
/// </summary>
public interface IEventoService
{
    Task<IEnumerable<Evento>> GetAllAsync();
    Task<Evento?> GetByIdAsync(int id);
    Task<Evento> CreateAsync(Evento evento);
    Task<Evento?> UpdateAsync(int id, Evento evento);
    Task<bool> DeleteAsync(int id);
}
