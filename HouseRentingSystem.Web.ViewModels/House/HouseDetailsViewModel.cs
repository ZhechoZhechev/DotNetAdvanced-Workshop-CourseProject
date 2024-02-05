namespace HouseRentingSystem.Web.ViewModels.House;

using HouseRentingSystem.Web.ViewModels.Agent;

public class HouseDetailsViewModel : AllHousesViewModel
{
    public string Description { get; set; } = null!;

    public string Category { get; set; } = null!;

    public AgentInfoViewModel Agent { get; set; } = null!;
}
