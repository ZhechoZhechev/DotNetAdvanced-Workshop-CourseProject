namespace HouseRentingSystem.WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;

using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

[Route("api/statistics")]
[ApiController]
public class StatisticsApiController : ControllerBase
{
    private readonly IStatisticService statisticService;

    public StatisticsApiController(IStatisticService statisticService)
    {
        this.statisticService = statisticService;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200, Type = typeof(StatisticsServiceModel))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetStatistics()
    {
        try
        {
            var model = await statisticService.GetStatistics();

            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
