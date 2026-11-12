using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.DTOs;
using TodoApi.Application.Interfaces;

namespace TodoApi.Api.Controllers;

[ApiController]
[Route("api/tarefas")]
[Produces("application/json")]
[Tags("Tarefas")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaServico _tarefaServico;

    public TarefasController(ITarefaServico tarefaServico)
    {
        _tarefaServico = tarefaServico;
    }

    /// <summary>
    /// Lista tarefas com filtros opcionais por status e data de vencimento.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TarefaDto>>> Listar(
        [FromQuery] FiltroTarefa filtro,
        CancellationToken cancellationToken)
    {
        var tarefas = await _tarefaServico.ListarAsync(filtro, cancellationToken);
        return Ok(tarefas);
    }

    /// <summary>
    /// Busca uma tarefa pelo ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TarefaDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var tarefa = await _tarefaServico.ObterPorIdAsync(id, cancellationToken);
        if (tarefa is null)
        {
            return NotFound();
        }

        return Ok(tarefa);
    }

    /// <summary>
    /// Cria uma nova tarefa.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TarefaDto>> Criar(
        [FromBody] CriarTarefaRequest request,
        CancellationToken cancellationToken)
    {
        var criada = await _tarefaServico.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
    }

    /// <summary>
    /// Atualiza uma tarefa existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TarefaDto>> Atualizar(
        Guid id,
        [FromBody] AtualizarTarefaRequest request,
        CancellationToken cancellationToken)
    {
        var atualizada = await _tarefaServico.AtualizarAsync(id, request, cancellationToken);
        if (atualizada is null)
        {
            return NotFound();
        }

        return Ok(atualizada);
    }

    /// <summary>
    /// Remove uma tarefa.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid id, CancellationToken cancellationToken)
    {
        var removida = await _tarefaServico.RemoverAsync(id, cancellationToken);
        if (!removida)
        {
            return NotFound();
        }

        return NoContent();
    }
}
