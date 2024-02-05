namespace HouseRentingSystem.Services.Interfaces;

using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

public interface IHouseService
{
    Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();

    Task CreateHouse(HouseFormModel model, string agentId);

    Task<HousesQueryServiceModel> AllAsync(AllHousesQueryModel queryModel);

    Task<ICollection<AllHousesViewModel>> AllHousesByAgentIdAsync(string agentId);

    Task<ICollection<AllHousesViewModel>> AllHousesByUserIdAsync(string userId);

    Task<bool> HouseExistsByIdAsync(string houseId);

    Task<HouseDetailsViewModel> HouseDetailsByIdAsync(string houseId);
}
