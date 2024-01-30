namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;

public class HouseService : IHouseService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public HouseService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
    {
        var lastThreeHouses = await this.dbContext.Houses
            .OrderByDescending(h => h.CreatedOn)
            .Take(3)
            .Select(h => new IndexViewModel
            {
                Id = h.Id.ToString(),
                Title = h.Title,
                ImageUrl = h.ImageUrl
            })
            .ToArrayAsync();

        return lastThreeHouses;
    }
}
