namespace HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Services.Interfaces;
using static HouseRentingSystem.Common.NotificationsMessages;

[Authorize]
public class AgentController : Controller
{
    private readonly IAgentService agentService;

    public AgentController(IAgentService agentService)
    {
        this.agentService = agentService;
    }

    [HttpGet]
    public async Task<IActionResult> Become() 
    {
        var userId =  this.User.GetId();
        var isAgent = await agentService.AgentExistsByUserId(userId);
        if (isAgent) 
        {
            TempData[ErrorMessage] = "You are already an agent!";

            return RedirectToAction("Index", "Home");
        }

        return View();
    }
}
 