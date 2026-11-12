using TodoApi.Domain.Enums;

namespace TodoApi.Domain.Entities;

public class Tarefa
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public StatusTarefa Status { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }

    private Tarefa()
    {
    }

    public Tarefa(string titulo, string descricao, DateTime dataVencimento)
    {
        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        Descricao = descricao.Trim();
        Status = StatusTarefa.Pendente;
        DataVencimento = dataVencimento.Date;
        DataCriacao = DateTime.UtcNow;
    }

    public void Atualizar(string titulo, string descricao, StatusTarefa status, DateTime dataVencimento)
    {
        Titulo = titulo.Trim();
        Descricao = descricao.Trim();
        Status = status;
        DataVencimento = dataVencimento.Date;
        DataAtualizacao = DateTime.UtcNow;
    }
}
