namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

public class StatisticService : IStatisticService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public StatisticService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<StatisticsServiceModel> GetStatistics()
    {
        var allHouses = await dbContext.Houses.CountAsync();
        var rentedHouses = await dbContext.Houses
            .Where(h => h.IsActive && h.RenterId.HasValue)
            .CountAsync();

        return new StatisticsServiceModel()
        {
            TotalHouses = allHouses,
            TotalRents = rentedHouses
        };
    }
}
