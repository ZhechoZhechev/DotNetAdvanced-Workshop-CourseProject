namespace HouseRentingSystem.Web.ViewModels.Agent;

using System.ComponentModel.DataAnnotations;

using static HouseRentingSystem.Common.EntityValidationConstants.AgentConstants;

public class BecomeAgentFormModel
{
    [Phone]
    [Required]
    [Display(Name = "Phone number")]
    [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
    public string PhoneNumber { get; set; } = null!;
}
