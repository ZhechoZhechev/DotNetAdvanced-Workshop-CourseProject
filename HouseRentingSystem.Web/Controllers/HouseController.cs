namespace HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House;
using static HouseRentingSystem.Common.NotificationsMessages;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

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
    public async Task<IActionResult> All([FromQuery]AllHousesQueryModel houseQueryModel)
    {
        HousesQueryServiceModel allHousesSrviceModel = await this.houseService.AllAsync(houseQueryModel);

        houseQueryModel.TotalHouses = allHousesSrviceModel.TotalHousesCount;
        houseQueryModel.Houses = allHousesSrviceModel.Houses;
        houseQueryModel.Categories = await this.categoryService.AllCategoryNamesAsync();

        return View(houseQueryModel);
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

        if (await categoryService.IfCategotyExistsAsync(model.CategoryId) == false)
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

    [HttpGet]
    public async Task<IActionResult> Mine() 
    {
        IEnumerable<AllHousesViewModel> allHouses;

        var userId = this.User.GetId();

        if (await agentService.AgentExistsByUserIdAsync(userId)) 
        {
            allHouses = await houseService.AllHousesByAgentIdAsync(userId);
        }
        else
        {
            allHouses = await houseService.AllHousesByUserIdAsync(userId);
        }

        return View(allHouses);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(string id) 
    {
        var houseExists = await houseService.HouseExistsByIdAsync(id);

        if (!houseExists)
        {
            TempData[InfoMessage] = "House does not exist!";
            return RedirectToAction("All", "House");
        }

        var houseModel = await houseService.HouseDetailsByIdAsync(id);

        return View(houseModel);
    }
}
