using FluentAssertions;
using TodoApi.Domain.Entities;
using TodoApi.Domain.Enums;

namespace TodoApi.UnitTests.Domain;

public class TarefaTests
{
    [Fact]
    public void Construtor_DeveInicializarPadroes()
    {
        var dataVencimento = new DateTime(2026, 11, 30, 15, 45, 0);

        var tarefa = new Tarefa("  Revisar PR  ", "  Ajustar documentação  ", dataVencimento);

        tarefa.Id.Should().NotBeEmpty();
        tarefa.Titulo.Should().Be("Revisar PR");
        tarefa.Descricao.Should().Be("Ajustar documentação");
        tarefa.Status.Should().Be(StatusTarefa.Pendente);
        tarefa.DataVencimento.Should().Be(dataVencimento.Date);
        tarefa.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        tarefa.DataAtualizacao.Should().BeNull();
    }

    [Fact]
    public void Atualizar_DeveAlterarCamposEPreencherDataAtualizacao()
    {
        var tarefa = new Tarefa("Antes", "Desc", new DateTime(2026, 11, 10));

        tarefa.Atualizar("Depois", "Nova descrição", StatusTarefa.Concluido, new DateTime(2026, 12, 1, 8, 0, 0));

        tarefa.Titulo.Should().Be("Depois");
        tarefa.Descricao.Should().Be("Nova descrição");
        tarefa.Status.Should().Be(StatusTarefa.Concluido);
        tarefa.DataVencimento.Should().Be(new DateTime(2026, 12, 1));
        tarefa.DataAtualizacao.Should().NotBeNull();
    }
}
