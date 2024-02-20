namespace HouseRentingSystem.Web.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.User;

using static HouseRentingSystem.Common.GeneralApplicationConstants;
public class UserController : BaseAdminController
{
    private readonly IUserService userService;
    private readonly IMemoryCache memoryCache;

    public UserController(IUserService userService, IMemoryCache memoryCache)
    {
        this.userService = userService;
        this.memoryCache = memoryCache;
    }

    [ResponseCache(Duration = 30)]
    public async Task<IActionResult> All()
    {
        var allUsersModel = memoryCache.Get<IEnumerable<UserViewModel>>(UsersCacheKey);

        if (allUsersModel == null)
        {
            allUsersModel = await userService.AllUsersAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(UserMemoryCacheExpDuration));

            memoryCache.Set(UsersCacheKey, allUsersModel, cacheOptions);
        }


        return View(allUsersModel);
    }
}
