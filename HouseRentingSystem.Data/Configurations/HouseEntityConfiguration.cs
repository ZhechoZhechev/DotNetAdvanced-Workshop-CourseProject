namespace HouseRentingSystem.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HouseRentingSystem.Data.Models;

public class HouseEntityConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder
            .HasOne(c => c.Category)
            .WithMany(h => h.Houses)
            .HasForeignKey(h => h.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(a => a.Agent)
            .WithMany(h => h.ManagedHouses)
            .HasForeignKey(h => h.AgentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
