using System.Text.Json.Serialization;

namespace TodoApi.Domain.Enums;

public enum StatusTarefa
{
    [JsonStringEnumMemberName("Pendente")]
    Pendente = 0,

    [JsonStringEnumMemberName("Em andamento")]
    EmAndamento = 1,

    [JsonStringEnumMemberName("Concluído")]
    Concluido = 2
}
