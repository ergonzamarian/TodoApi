using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.DTOs;
using TodoApi.Domain.Entities;
using TodoApi.Domain.Enums;
using TodoApi.Infrastructure.Persistence;
using TodoApi.Infrastructure.Repositories;

namespace TodoApi.UnitTests.Repositories;

public class TarefaRepositorioTests
{
    private static AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ListarAsync_DeveFiltrarPorStatusEDataVencimento()
    {
        await using var contexto = CriarContexto();
        var repositorio = new TarefaRepositorio(contexto);

        var pendente = new Tarefa("Pendente", "A", new DateTime(2026, 11, 10));

        var emAndamento = new Tarefa("Em andamento", "B", new DateTime(2026, 11, 20));
        emAndamento.Atualizar(emAndamento.Titulo, emAndamento.Descricao, StatusTarefa.EmAndamento, emAndamento.DataVencimento);

        var outroDia = new Tarefa("Outro dia", "C", new DateTime(2026, 11, 21));
        outroDia.Atualizar(outroDia.Titulo, outroDia.Descricao, StatusTarefa.EmAndamento, outroDia.DataVencimento);

        await repositorio.AdicionarAsync(pendente);
        await repositorio.AdicionarAsync(emAndamento);
        await repositorio.AdicionarAsync(outroDia);
        await contexto.SaveChangesAsync();

        var filtro = new FiltroTarefa
        {
            Status = StatusTarefa.EmAndamento,
            DataVencimento = new DateTime(2026, 11, 20)
        };

        var resultado = await repositorio.ListarAsync(filtro);

        resultado.Should().HaveCount(1);
        resultado[0].Titulo.Should().Be("Em andamento");
        resultado[0].Status.Should().Be(StatusTarefa.EmAndamento);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarTarefaPersistida()
    {
        await using var contexto = CriarContexto();
        var repositorio = new TarefaRepositorio(contexto);
        var tarefa = new Tarefa("Buscar", "Detalhe", new DateTime(2026, 11, 18));

        await repositorio.AdicionarAsync(tarefa);
        await contexto.SaveChangesAsync();

        var encontrada = await repositorio.ObterPorIdAsync(tarefa.Id);

        encontrada.Should().NotBeNull();
        encontrada!.Titulo.Should().Be("Buscar");
    }
}
