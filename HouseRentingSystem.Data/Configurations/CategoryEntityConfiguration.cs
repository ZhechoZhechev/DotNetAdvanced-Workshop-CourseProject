using HouseRentingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseRentingSystem.Data.Configurations;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasData(this.GenerateCategories());
    }

    private ICollection<Category> GenerateCategories()
    {
        var result = new List<Category>()
        {
            new Category
            {
                Id = 1,
                Name = "Cottage"
            },
            new Category
            {
                Id= 2,
                Name = "Single-Family"
            },
            new Category
            {
                Id = 3,
                Name = "Duplex"
            }
        };

        return result;
    }
}
