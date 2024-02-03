using HouseRentingSystem.Web.ViewModels.Category;

namespace HouseRentingSystem.Services.Interfaces;

public interface ICategoryService
{
    Task<ICollection<HouseCategoryFormModel>> AllHouseCategoriesAsync();

    Task<bool> IfCategotyExistsAsync(int categotyId);

    Task<ICollection<string>> AllCategoryNamesAsync();
}
