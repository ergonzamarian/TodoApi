using TodoApi.Application.Interfaces;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Infrastructure.Repositories;

public class UnidadeDeTrabalho : IUnidadeDeTrabalho
{
    private readonly AppDbContext _contexto;

    public UnidadeDeTrabalho(AppDbContext contexto, ITarefaRepositorio tarefas)
    {
        _contexto = contexto;
        Tarefas = tarefas;
    }

    public ITarefaRepositorio Tarefas { get; }

    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default)
    {
        return _contexto.SaveChangesAsync(cancellationToken);
    }
}
