using InventoryControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryControl.Infrastructure.Context.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovement");

            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Observation)
                .HasMaxLength(255);

            builder.Property(sm => sm.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(sm => sm.Product)
                .WithMany(p => p.StockMovements)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.User)
                .WithMany(u => u.StockMovements)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
