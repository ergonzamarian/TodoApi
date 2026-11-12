using TodoApi.Application.DTOs;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Interfaces;

public interface ITarefaRepositorio
{
    Task<Tarefa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tarefa>> ListarAsync(FiltroTarefa filtro, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Tarefa tarefa, CancellationToken cancellationToken = default);
    void Atualizar(Tarefa tarefa);
    void Remover(Tarefa tarefa);
}
