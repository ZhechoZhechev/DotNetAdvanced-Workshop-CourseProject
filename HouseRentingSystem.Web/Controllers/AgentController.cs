namespace HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Agent;
using static HouseRentingSystem.Common.NotificationsMessages;

[Authorize]
public class AgentController : Controller
{
    private readonly IAgentService agentService;

    public AgentController(IAgentService agentService)
    {
        this.agentService = agentService;
    }

    /// <summary>
    /// Used to become and agent. If you are already an agent toastr JS library used to pop a message.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Become()
    {
        var isAgent = await this.CheckIfUserIsAgentAsync();
        if (isAgent) this.DisplayMessagAndRedirect();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Become(BecomeAgentFormModel model)
    {
        var isAgent = await this.CheckIfUserIsAgentAsync();
        if (isAgent) this.DisplayMessagAndRedirect();

        var isPhoneNumberTaken = await agentService.AgentExistsByPhoneNumberAsync(model.PhoneNumber);
        if (isPhoneNumberTaken)
            this.ModelState.AddModelError(nameof(model.PhoneNumber), "Agent with this phone number already exists!");

        if (!this.ModelState.IsValid) return View(model);

        var hasAnyRentedHouses = await agentService.UserHasRentsAsync(this.User.GetId());
        if (hasAnyRentedHouses)
        {
            TempData[ErrorMessage] = "You cannot have rented houses and become and agent!";
            return RedirectToAction("Mine", "House");
        }

        try
        {
            await this.agentService.CreateAgentAsync(this.User.GetId(), model);
        }
        catch (Exception)
        {

            TempData[ErrorMessage] = "Unexpected error accured. Try again!";
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("All", "Houses");
    }

    private async Task<bool> CheckIfUserIsAgentAsync()
    {
        var userId = this.User.GetId();
        var isAgent = await agentService.AgentExistsByUserIdAsync(userId);
        return isAgent;

    }

    private void DisplayMessagAndRedirect()
    {
        TempData[ErrorMessage] = "You are already an agent!";
        RedirectToAction("Index", "Home");
    }
}
