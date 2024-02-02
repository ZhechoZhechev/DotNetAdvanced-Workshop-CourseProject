using HouseRentingSystem.Web.ViewModels.Category;

namespace HouseRentingSystem.Services.Interfaces;

public interface ICategoryService
{
    Task<ICollection<HouseCategoryFormModel>> AllHouseCategoriesAsync();
}
