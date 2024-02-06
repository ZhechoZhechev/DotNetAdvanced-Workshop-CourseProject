namespace HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;
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
    public async Task<IActionResult> All([FromQuery] AllHousesQueryModel houseQueryModel)
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

        string houseId;
        try
        {
            var agentId = await agentService.AgentIdByUserIdAsync(this.User.GetId());
            houseId = await this.houseService.CreateHouseAndReturnHouseIdAsync(model, agentId!);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error occured while creating your house. Please, try again.");
            model.Categories = await categoryService.AllHouseCategoriesAsync();

            return View(model);
        }

        return RedirectToAction("Details", "House", new { id = houseId });
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

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var houseExists = await houseService.HouseExistsByIdAsync(id);
        if (!houseExists)
        {
            TempData[InfoMessage] = "House does not exist!";
            return RedirectToAction("All", "House");
        }

        var isUserAgent = await agentService.AgentExistsByUserIdAsync(this.User.GetId());
        if (!isUserAgent)
        {
            TempData[InfoMessage] = "You are not an agent! Became agent first.";
            return RedirectToAction("Become", "Agent");
        }

        var agentId = await agentService.AgentIdByUserIdAsync(this.User.GetId());
        var isAgentHouseOwner = await houseService.IsAgentWithIdOwnerHouseWithIdAsync(id, agentId!);
        if (!isAgentHouseOwner)
        {
            TempData[InfoMessage] = "You are not owner if this house!";
            return RedirectToAction("Mine", "House");
        }

        var houseFormModel = await houseService.GetHouseForEditAsync(id);
        houseFormModel.Categories = await categoryService.AllHouseCategoriesAsync();
        return View(houseFormModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, HouseFormModel houseModel)
    {
        if (!ModelState.IsValid)
        {
            houseModel.Categories = await categoryService.AllHouseCategoriesAsync();
            return View(houseModel);
        }

        var houseExists = await houseService.HouseExistsByIdAsync(id);
        if (!houseExists)
        {
            TempData[InfoMessage] = "House does not exist!";
            return RedirectToAction("All", "House");
        }

        var isUserAgent = await agentService.AgentExistsByUserIdAsync(this.User.GetId());
        if (!isUserAgent)
        {
            TempData[InfoMessage] = "You are not an agent! Became agent first.";
            return RedirectToAction("Become", "Agent");
        }

        var agentId = await agentService.AgentIdByUserIdAsync(this.User.GetId());
        var isAgentHouseOwner = await houseService.IsAgentWithIdOwnerHouseWithIdAsync(id, agentId!);
        if (!isAgentHouseOwner)
        {
            TempData[InfoMessage] = "You are not owner if this house!";
            return RedirectToAction("Mine", "House");
        }


        try
        {
            await houseService.EditHouseByIdAsync(id, houseModel);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error occured while editing your house. Please, try again.");
            houseModel.Categories = await categoryService.AllHouseCategoriesAsync();

            return View(houseModel);
        }

        TempData[SuccessMessage] = "House was edited successfully!";
        return RedirectToAction("Details", "House", new { id });
    }
}
