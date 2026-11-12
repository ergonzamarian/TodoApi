using Microsoft.EntityFrameworkCore;
using TodoApi.Domain.Entities;

namespace TodoApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>(entidade =>
        {
            entidade.ToTable("Tarefas");
            entidade.HasKey(tarefa => tarefa.Id);

            entidade.Property(tarefa => tarefa.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            entidade.Property(tarefa => tarefa.Descricao)
                .HasMaxLength(2000);

            entidade.Property(tarefa => tarefa.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entidade.Property(tarefa => tarefa.DataVencimento)
                .IsRequired();

            entidade.Property(tarefa => tarefa.DataCriacao)
                .IsRequired();

            entidade.HasIndex(tarefa => tarefa.Status);
            entidade.HasIndex(tarefa => tarefa.DataVencimento);
        });
    }
}
