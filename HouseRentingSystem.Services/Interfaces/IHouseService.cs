namespace HouseRentingSystem.Services.Interfaces;

using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

public interface IHouseService
{
    Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();

    Task<string> CreateHouseAndReturnHouseIdAsync(HouseFormModel model, string agentId);

    Task<HousesQueryServiceModel> AllAsync(AllHousesQueryModel queryModel);

    Task<ICollection<AllHousesViewModel>> AllHousesByAgentIdAsync(string agentId);

    Task<ICollection<AllHousesViewModel>> AllHousesByUserIdAsync(string userId);

    Task<bool> HouseExistsByIdAsync(string houseId);

    Task<HouseDetailsViewModel> HouseDetailsByIdAsync(string houseId);

    Task<HouseFormModel> GetHouseForEditAsync(string houseId);

    Task<bool> IsAgentWithIdOwnerHouseWithIdAsync(string houseId, string agentId);

    Task EditHouseByIdAsync(string houseId, HouseFormModel model);

    Task<HouseDeleteViewModel> GetHouseForDeletionAsync(string houseId);

    Task DeleteHouseByIdAsync(string houseId);

    Task<bool> IsHouseRentedAsync(string houseId);

    Task<bool> IsUserHouseRentierAsync(string houseId, string userId);

    Task RentHouse(string houseId, string userId);

    Task LeaveHouse(string houseId);

}
