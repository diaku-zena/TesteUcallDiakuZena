using Microsoft.EntityFrameworkCore;
using Revisao.Domain.Entities;

namespace Revisao.Infra.Context;

public class RevisaoContext : DbContext
{
    public RevisaoContext(DbContextOptions<RevisaoContext> options) : base(options)
    { }

    public DbSet<OrderItem> OrderItems{ get; set; }
    public DbSet<Order> Orders{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações adicionais de mapeamento
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId);
    }

}
