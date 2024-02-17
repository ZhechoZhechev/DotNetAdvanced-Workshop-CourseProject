namespace HouseRentingSystem.Services.Interfaces;

using HouseRentingSystem.Web.ViewModels.User;

public interface IUserService
{
    Task<string> GetFullNameByUserIdAsync(string userId);

    Task<IEnumerable<UserViewModel>> AllUsersAsync();
}
