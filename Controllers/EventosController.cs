using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

/// <summary>
/// Controlador REST para la administración de eventos tecnológicos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventosController : ControllerBase
{
    private readonly IEventoService _eventoService;

    public EventosController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }

    /// <summary>
    /// Obtiene todos los eventos registrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Evento>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Evento>>> GetAll()
    {
        var eventos = await _eventoService.GetAllAsync();
        return Ok(eventos);
    }

    /// <summary>
    /// Obtiene un evento específico por su identificador.
    /// </summary>
    /// <param name="id">Identificador del evento.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Evento), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Evento>> GetById(int id)
    {
        var evento = await _eventoService.GetByIdAsync(id);
        if (evento is null)
        {
            return NotFound(new { mensaje = $"No se encontró el evento con Id {id}." });
        }

        return Ok(evento);
    }

    /// <summary>
    /// Crea un nuevo evento tecnológico.
    /// </summary>
    /// <param name="evento">Datos del evento a registrar.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Evento), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Evento>> Create([FromBody] Evento evento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var creado = await _eventoService.CreateAsync(evento);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un evento existente.
    /// </summary>
    /// <param name="id">Identificador del evento a actualizar.</param>
    /// <param name="evento">Nuevos datos del evento.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Evento), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Evento>> Update(int id, [FromBody] Evento evento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var actualizado = await _eventoService.UpdateAsync(id, evento);
            if (actualizado is null)
            {
                return NotFound(new { mensaje = $"No se encontró el evento con Id {id}." });
            }

            return Ok(actualizado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un evento por su identificador.
    /// </summary>
    /// <param name="id">Identificador del evento a eliminar.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _eventoService.DeleteAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No se encontró el evento con Id {id}." });
        }

        return NoContent();
    }
}
