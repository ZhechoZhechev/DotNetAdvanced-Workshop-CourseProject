namespace HouseRentingSystem.Services.Interfaces;

using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

public interface IStatisticService
{
    Task<StatisticsServiceModel> GetStatistics();
}
