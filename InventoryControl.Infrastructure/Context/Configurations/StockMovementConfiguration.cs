using InventoryControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryControl.Infrastructure.Context.Configurations
{
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovements");

            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Observation)
                .HasMaxLength(255);

            builder.Property(sm => sm.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(sm => sm.Product)
                .WithMany(p => p.StockMovements)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sm => sm.User)
                .WithMany()
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
