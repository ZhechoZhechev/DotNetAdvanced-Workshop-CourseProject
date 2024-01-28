namespace HouseRentingSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static HouseRentingSystem.Common.EntityValidationConstants.AgentConstants;

public class Agent
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
}