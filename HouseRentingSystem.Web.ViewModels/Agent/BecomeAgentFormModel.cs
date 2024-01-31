namespace HouseRentingSystem.Web.ViewModels.Agent;

using System.ComponentModel.DataAnnotations;

public class BecomeAgentFormModel
{
    [Phone]
    public string PhoneNumber { get; set; }
}
