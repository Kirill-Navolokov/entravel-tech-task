using Microsoft.EntityFrameworkCore;
using OrderProcessingService.DAL.Entities;

namespace OrderProcessingService.DAL;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<OrderItemEntity> OrderItems { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<OrderEntity>()
            .HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderEntity>()
            .Property(o => o.Id)
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd(); ;

        modelBuilder.Entity<OrderItemEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<OrderItemEntity>()
            .Property(e => e.Id)
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<UserEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<UserEntity>()
            .Property(e => e.Id)
            .HasColumnType("uuid");
        
        modelBuilder.Entity<UserEntity>()
            .HasMany(o => o.Orders)
            .WithOne(i => i.Customer)
            .HasForeignKey(i => i.CustomerId);
    }
}