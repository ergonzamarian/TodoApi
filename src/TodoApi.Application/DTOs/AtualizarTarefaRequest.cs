using System.ComponentModel.DataAnnotations;
using TodoApi.Domain.Enums;

namespace TodoApi.Application.DTOs;

public class AtualizarTarefaRequest
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "O título deve ter entre 1 e 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status é obrigatório.")]
    [EnumDataType(typeof(StatusTarefa), ErrorMessage = "Status inválido.")]
    public StatusTarefa Status { get; set; }

    [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
    public DateTime DataVencimento { get; set; }
}
