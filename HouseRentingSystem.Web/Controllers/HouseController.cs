namespace HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House;
using static HouseRentingSystem.Common.NotificationsMessages;

[Authorize]
public class HouseController : Controller
{
    private readonly ICategoryService categoryService;
    private readonly IAgentService agentService;

    public HouseController(ICategoryService categoryService, IAgentService agentService)
    {
        this.categoryService = categoryService;
        this.agentService = agentService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        if (await agentService.AgentExistsByUserIdAsync(this.User.GetId()) == false)
        {
            TempData[InfoMessage] = "You must be an angent to add a house!";
            return RedirectToAction("Become", "Agent");
        }

        var houseModel = new HouseFormModel()
        {
            Categories = await categoryService.AllHouseCategoriesAsync(),
        };

        return View(houseModel);
    }
}
