using Microsoft.EntityFrameworkCore;
using TodoApi.Application.DTOs;
using TodoApi.Application.Interfaces;
using TodoApi.Domain.Entities;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Infrastructure.Repositories;

public class TarefaRepositorio : ITarefaRepositorio
{
    private readonly AppDbContext _contexto;

    public TarefaRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Tarefa?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _contexto.Tarefas
            .FirstOrDefaultAsync(tarefa => tarefa.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Tarefa>> ListarAsync(
        FiltroTarefa filtro,
        CancellationToken cancellationToken = default)
    {
        var consulta = _contexto.Tarefas.AsQueryable();

        if (filtro.Status.HasValue)
        {
            consulta = consulta.Where(tarefa => tarefa.Status == filtro.Status.Value);
        }

        if (filtro.DataVencimento.HasValue)
        {
            var dia = filtro.DataVencimento.Value.Date;
            consulta = consulta.Where(tarefa => tarefa.DataVencimento == dia);
        }

        return await consulta
            .OrderBy(tarefa => tarefa.DataVencimento)
            .ThenBy(tarefa => tarefa.Titulo)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Tarefa tarefa, CancellationToken cancellationToken = default)
    {
        await _contexto.Tarefas.AddAsync(tarefa, cancellationToken);
    }

    public void Atualizar(Tarefa tarefa)
    {
        _contexto.Tarefas.Update(tarefa);
    }

    public void Remover(Tarefa tarefa)
    {
        _contexto.Tarefas.Remove(tarefa);
    }
}
