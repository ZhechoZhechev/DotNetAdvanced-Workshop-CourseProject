namespace HouseRentingSystem.Services.Interfaces;

using HouseRentingSystem.Web.ViewModels.Home;

public interface IHouseService
{
    Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();
}
