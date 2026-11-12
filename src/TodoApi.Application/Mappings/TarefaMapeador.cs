using TodoApi.Application.DTOs;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Mappings;

public static class TarefaMapeador
{
    public static TarefaDto ParaDto(this Tarefa tarefa) =>
        new(
            tarefa.Id,
            tarefa.Titulo,
            tarefa.Descricao,
            tarefa.Status,
            tarefa.DataVencimento);
}
