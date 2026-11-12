using FluentAssertions;
using Moq;
using TodoApi.Application.DTOs;
using TodoApi.Application.Interfaces;
using TodoApi.Application.Services;
using TodoApi.Domain.Entities;
using TodoApi.Domain.Enums;

namespace TodoApi.UnitTests.Services;

public class TarefaServicoTests
{
    private readonly Mock<IUnidadeDeTrabalho> _unidadeMock = new();
    private readonly Mock<ITarefaRepositorio> _repositorioMock = new();
    private readonly TarefaServico _servico;

    public TarefaServicoTests()
    {
        _unidadeMock.Setup(u => u.Tarefas).Returns(_repositorioMock.Object);
        _servico = new TarefaServico(_unidadeMock.Object);
    }

    [Fact]
    public async Task CriarAsync_DevePersistirERetornarDto()
    {
        var request = new CriarTarefaRequest
        {
            Titulo = "Estudar Clean Architecture",
            Descricao = "Revisar camadas",
            DataVencimento = new DateTime(2026, 11, 20)
        };

        Tarefa? salva = null;

        _repositorioMock
            .Setup(r => r.AdicionarAsync(It.IsAny<Tarefa>(), It.IsAny<CancellationToken>()))
            .Callback<Tarefa, CancellationToken>((tarefa, _) => salva = tarefa)
            .Returns(Task.CompletedTask);

        _unidadeMock
            .Setup(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var resultado = await _servico.CriarAsync(request);

        resultado.Titulo.Should().Be(request.Titulo);
        resultado.Descricao.Should().Be(request.Descricao);
        resultado.Status.Should().Be(StatusTarefa.Pendente);
        resultado.DataVencimento.Should().Be(request.DataVencimento.Date);
        resultado.Id.Should().NotBeEmpty();
        salva.Should().NotBeNull();
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        var id = Guid.NewGuid();
        _repositorioMock
            .Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tarefa?)null);

        var resultado = await _servico.ObterPorIdAsync(id);

        resultado.Should().BeNull();
    }

    [Fact]
    public async Task AtualizarAsync_QuandoExiste_DeveAplicarAlteracoes()
    {
        var tarefa = new Tarefa("Antigo", "Desc", new DateTime(2026, 11, 15));
        var request = new AtualizarTarefaRequest
        {
            Titulo = "Novo",
            Descricao = "Nova desc",
            Status = StatusTarefa.EmAndamento,
            DataVencimento = new DateTime(2026, 11, 25)
        };

        _repositorioMock
            .Setup(r => r.ObterPorIdAsync(tarefa.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefa);

        _unidadeMock
            .Setup(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var resultado = await _servico.AtualizarAsync(tarefa.Id, request);

        resultado.Should().NotBeNull();
        resultado!.Titulo.Should().Be("Novo");
        resultado.Status.Should().Be(StatusTarefa.EmAndamento);
        resultado.DataVencimento.Should().Be(new DateTime(2026, 11, 25));
    }

    [Fact]
    public async Task RemoverAsync_QuandoExiste_DeveRetornarTrue()
    {
        var tarefa = new Tarefa("Remover", "Teste", DateTime.UtcNow.Date);

        _repositorioMock
            .Setup(r => r.ObterPorIdAsync(tarefa.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefa);

        _unidadeMock
            .Setup(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var resultado = await _servico.RemoverAsync(tarefa.Id);

        resultado.Should().BeTrue();
        _repositorioMock.Verify(r => r.Remover(tarefa), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_QuandoNaoExiste_DeveRetornarFalse()
    {
        var id = Guid.NewGuid();
        _repositorioMock
            .Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tarefa?)null);

        var resultado = await _servico.RemoverAsync(id);

        resultado.Should().BeFalse();
        _repositorioMock.Verify(r => r.Remover(It.IsAny<Tarefa>()), Times.Never);
    }
}
