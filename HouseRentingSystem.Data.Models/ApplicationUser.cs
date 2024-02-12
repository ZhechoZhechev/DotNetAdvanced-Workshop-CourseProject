namespace HouseRentingSystem.Data.Models;

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using static HouseRentingSystem.Common.EntityValidationConstants.UserConstants;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
        this.RentedHouses = new HashSet<House>();
    }

    [Required]
    [StringLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    public ICollection<House> RentedHouses { get; set; }
}
