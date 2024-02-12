namespace HouseRentingSystem.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HouseRentingSystem.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

public class HouseEntityConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder
            .Property(h => h.IsActive)
            .HasDefaultValue(true);

        builder
            .Property(h => h.CreatedOn)
            .HasDefaultValueSql("GETDATE()");

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

        builder
            .HasData(this.GenerateHouses());
    }

    private ICollection<House> GenerateHouses()
    {
        var result = new List<House>()
        {
            new House
            {
                Title = "Big House Marina",
                Address = "North London, UK (near the border)",
                Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                ImageUrl = "https://www.realestate.com.au/news-image/w_1280,h_720/v1674417684/news-lifestyle-content-assets/wp-content/production/capi_b35809130ee03e5138c24ca729bdc41c_977f7effd98be87c10477bd5c3fd1b3a.jpeg?_i=AA",
                PricePerMonth = 2100.00M,
                CategoryId = 3,
                AgentId = Guid.Parse("D686A26F-6DEB-48FD-899A-C8161CEDBFD4"),
                RenterId = Guid.Parse("6B515EEC-45FF-47BD-9805-08DC2191A455")
            },
            new House
            {
                 Title = "Family House Comfort",
                 Address = "Near the Sea Garden in Burgas, Bulgaria",
                 Description = "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.",
                 ImageUrl = "https://luxury-houses.net/wp-content/uploads/2021/07/Modern-Davids-House-for-Comfort-Family-Living-by-David-Small-Design-4.jpg",
                 PricePerMonth = 1200.00M,
                 CategoryId = 2,
                 AgentId = Guid.Parse("D686A26F-6DEB-48FD-899A-C8161CEDBFD4")
            },
            new House
            {
                 Title = "Grand House",
                 Address = "Boyana Neighbourhood, Sofia, Bulgaria",
                 Description = "This luxurious house is everything you will need. It is just excellent.",
                 ImageUrl = "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg",
                 PricePerMonth = 2000.00M,
                 CategoryId = 2,
                 AgentId = Guid.Parse("D686A26F-6DEB-48FD-899A-C8161CEDBFD4")
            }
        };

        return result;
    }
}
