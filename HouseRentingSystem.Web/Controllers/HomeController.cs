
namespace HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Services.Interfaces;

public class HomeController : Controller
{
    private readonly IHouseService houseService;

    public HomeController(IHouseService houseService)
    {
        this.houseService = houseService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await houseService.LastThreeHousesAsync();

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        if (statusCode == 400 ||statusCode == 404)
        {
            return View("Error404");
        }
        else if (statusCode == 401)
        {
            return View("Error401");
        }

        return View();
    }
}
