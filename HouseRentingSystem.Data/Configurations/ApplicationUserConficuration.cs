namespace HouseRentingSystem.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HouseRentingSystem.Data.Models;

public class ApplicationUserConficuration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(p => p.FirstName)
            .HasDefaultValue("FirstNTest");

        builder.Property(p => p.LastName)
            .HasDefaultValue("LastNTest");

    }
}
