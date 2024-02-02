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
    private readonly IHouseService houseService;
    public HouseController(
        ICategoryService categoryService,
        IAgentService agentService,
        IHouseService houseService)
    {
        this.categoryService = categoryService;
        this.agentService = agentService;
        this.houseService = houseService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> All()
    {
        return Ok();
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

    [HttpPost]
    public async Task<IActionResult> Add(HouseFormModel model)
    {
        if (await agentService.AgentExistsByUserIdAsync(this.User.GetId()) == false)
        {
            TempData[InfoMessage] = "You must be an angent to add a house!";
            return RedirectToAction("Become", "Agent");
        }

        if (await categoryService.IfCategotyExistsAsync(model.CategoryId))
        {
            ModelState.AddModelError(nameof(model.CategoryId), "Such categoty does not exist!");
        }

        if (!this.ModelState.IsValid)
        {
            model.Categories = await categoryService.AllHouseCategoriesAsync();
            return View(model);
        }

        try
        {
            var agentId = await agentService.AgentIdByUserIdAsync(this.User.GetId());
            await this.houseService.CreateHouse(model, agentId!);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error occured while creating your house. Please, try again.");
            model.Categories = await categoryService.AllHouseCategoriesAsync();

            return View(model);
        }

        return RedirectToAction("All", "House");
    }
}
