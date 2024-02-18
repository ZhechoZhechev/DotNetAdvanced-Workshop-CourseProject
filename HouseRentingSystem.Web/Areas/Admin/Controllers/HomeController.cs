using HouseRentingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public readonly IHouseService houseService;

        public HomeController(IHouseService houseService)
        {
            this.houseService = houseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllRented() 
        {
            var AllRentedViewModel = await houseService.AllRentedHousesAsync();

            return View(AllRentedViewModel);
        }
    }
}
