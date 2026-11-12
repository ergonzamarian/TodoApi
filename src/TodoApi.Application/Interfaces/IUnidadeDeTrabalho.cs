namespace TodoApi.Application.Interfaces;

public interface IUnidadeDeTrabalho
{
    ITarefaRepositorio Tarefas { get; }
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}
