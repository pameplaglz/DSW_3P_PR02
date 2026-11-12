using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

/// <summary>
/// Implementación del repositorio de eventos con persistencia en archivo JSON.
/// </summary>
public class JsonEventoRepository : IEventoRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public JsonEventoRepository(IWebHostEnvironment env)
    {
        var dataDir = Path.Combine(env.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDir);
        _filePath = Path.Combine(dataDir, "eventos.json");
    }

    private async Task<List<Evento>> ReadAllFromFileAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Evento>();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Evento>();
        }

        return JsonSerializer.Deserialize<List<Evento>>(json, JsonOptions) ?? new List<Evento>();
    }

    private async Task WriteAllToFileAsync(List<Evento> eventos)
    {
        var json = JsonSerializer.Serialize(eventos, JsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<IEnumerable<Evento>> GetAllAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return await ReadAllFromFileAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Evento?> GetByIdAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var eventos = await ReadAllFromFileAsync();
            return eventos.FirstOrDefault(e => e.Id == id);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Evento> AddAsync(Evento evento)
    {
        await _semaphore.WaitAsync();
        try
        {
            var eventos = await ReadAllFromFileAsync();
            evento.Id = eventos.Count == 0 ? 1 : eventos.Max(e => e.Id) + 1;
            eventos.Add(evento);
            await WriteAllToFileAsync(eventos);
            return evento;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Evento?> UpdateAsync(Evento evento)
    {
        await _semaphore.WaitAsync();
        try
        {
            var eventos = await ReadAllFromFileAsync();
            var existing = eventos.FirstOrDefault(e => e.Id == evento.Id);
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

            await WriteAllToFileAsync(eventos);
            return existing;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var eventos = await ReadAllFromFileAsync();
            var existing = eventos.FirstOrDefault(e => e.Id == id);
            if (existing is null)
            {
                return false;
            }

            eventos.Remove(existing);
            await WriteAllToFileAsync(eventos);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
