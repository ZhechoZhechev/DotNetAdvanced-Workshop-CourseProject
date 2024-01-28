namespace HouseRentingSystem.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using HouseRentingSystem.Data.Models;
using Microsoft.AspNetCore.Identity;

public class HouseRentingSystemDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public HouseRentingSystemDbContext(DbContextOptions<HouseRentingSystemDbContext> options)
        : base(options)
    {
    }
}
