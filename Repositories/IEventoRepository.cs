using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

/// <summary>
/// Contrato de acceso a datos para eventos tecnológicos.
/// </summary>
public interface IEventoRepository
{
    Task<IEnumerable<Evento>> GetAllAsync();
    Task<Evento?> GetByIdAsync(int id);
    Task<Evento> AddAsync(Evento evento);
    Task<Evento?> UpdateAsync(Evento evento);
    Task<bool> DeleteAsync(int id);
}
