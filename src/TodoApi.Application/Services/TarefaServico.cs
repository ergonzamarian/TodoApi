using TodoApi.Application.DTOs;
using TodoApi.Application.Interfaces;
using TodoApi.Application.Mappings;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Services;

public class TarefaServico : ITarefaServico
{
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public TarefaServico(IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task<TarefaDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tarefa = await _unidadeDeTrabalho.Tarefas.ObterPorIdAsync(id, cancellationToken);
        return tarefa?.ParaDto();
    }

    public async Task<IReadOnlyList<TarefaDto>> ListarAsync(
        FiltroTarefa filtro,
        CancellationToken cancellationToken = default)
    {
        var tarefas = await _unidadeDeTrabalho.Tarefas.ListarAsync(filtro, cancellationToken);
        return tarefas.Select(tarefa => tarefa.ParaDto()).ToList();
    }

    public async Task<TarefaDto> CriarAsync(
        CriarTarefaRequest request,
        CancellationToken cancellationToken = default)
    {
        var tarefa = new Tarefa(request.Titulo, request.Descricao, request.DataVencimento);

        await _unidadeDeTrabalho.Tarefas.AdicionarAsync(tarefa, cancellationToken);
        await _unidadeDeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        return tarefa.ParaDto();
    }

    public async Task<TarefaDto?> AtualizarAsync(
        Guid id,
        AtualizarTarefaRequest request,
        CancellationToken cancellationToken = default)
    {
        var tarefa = await _unidadeDeTrabalho.Tarefas.ObterPorIdAsync(id, cancellationToken);
        if (tarefa is null)
        {
            return null;
        }

        tarefa.Atualizar(request.Titulo, request.Descricao, request.Status, request.DataVencimento);
        _unidadeDeTrabalho.Tarefas.Atualizar(tarefa);
        await _unidadeDeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        return tarefa.ParaDto();
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tarefa = await _unidadeDeTrabalho.Tarefas.ObterPorIdAsync(id, cancellationToken);
        if (tarefa is null)
        {
            return false;
        }

        _unidadeDeTrabalho.Tarefas.Remover(tarefa);
        await _unidadeDeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        return true;
    }
}
