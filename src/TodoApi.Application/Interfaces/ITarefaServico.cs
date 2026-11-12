using TodoApi.Application.DTOs;

namespace TodoApi.Application.Interfaces;

public interface ITarefaServico
{
    Task<TarefaDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TarefaDto>> ListarAsync(FiltroTarefa filtro, CancellationToken cancellationToken = default);
    Task<TarefaDto> CriarAsync(CriarTarefaRequest request, CancellationToken cancellationToken = default);
    Task<TarefaDto?> AtualizarAsync(Guid id, AtualizarTarefaRequest request, CancellationToken cancellationToken = default);
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
