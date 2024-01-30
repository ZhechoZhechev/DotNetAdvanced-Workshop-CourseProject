namespace HouseRentingSystem.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using HouseRentingSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

public class HouseRentingSystemDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public HouseRentingSystemDbContext(DbContextOptions<HouseRentingSystemDbContext> options)
        : base(options)
    {
    }

    public DbSet<Agent> Agents { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<House> Houses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Assembly configAssembly = Assembly.GetAssembly(typeof(HouseRentingSystemDbContext)) ??
            Assembly.GetExecutingAssembly();
        builder.ApplyConfigurationsFromAssembly(configAssembly);

        base.OnModelCreating(builder);
    }
}
