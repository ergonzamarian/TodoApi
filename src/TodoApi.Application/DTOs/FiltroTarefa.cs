using TodoApi.Domain.Enums;

namespace TodoApi.Application.DTOs;

public class FiltroTarefa
{
    public StatusTarefa? Status { get; set; }
    public DateTime? DataVencimento { get; set; }
}
