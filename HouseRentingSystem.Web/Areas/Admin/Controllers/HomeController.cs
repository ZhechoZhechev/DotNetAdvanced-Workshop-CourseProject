namespace HouseRentingSystem.Web.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.House;

using static HouseRentingSystem.Common.GeneralApplicationConstants;


public class HomeController : BaseAdminController
{
    public readonly IHouseService houseService;
    private readonly IMemoryCache memoryCache;

    public HomeController(IHouseService houseService, IMemoryCache memoryCache)
    {
        this.houseService = houseService;
        this.memoryCache = memoryCache;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AllRented()
    {
        var AllRentedViewModel = memoryCache.Get<IEnumerable<RentedHousesViewModel>>(RentedHousesCacheKey);

        if (AllRentedViewModel == null)
        {
            AllRentedViewModel = await houseService.AllRentedHousesAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(RentedHousesMemoryCacheExpDuration));

            memoryCache.Set(RentedHousesCacheKey, AllRentedViewModel, cacheOptions);
        }

        return View(AllRentedViewModel);
    }
}
