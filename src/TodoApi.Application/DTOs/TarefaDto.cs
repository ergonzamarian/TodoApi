using TodoApi.Domain.Enums;

namespace TodoApi.Application.DTOs;

public record TarefaDto(
    Guid Id,
    string Titulo,
    string Descricao,
    StatusTarefa Status,
    DateTime DataVencimento);
