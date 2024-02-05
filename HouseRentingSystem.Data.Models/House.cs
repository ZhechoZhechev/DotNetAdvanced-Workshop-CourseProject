namespace HouseRentingSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HouseRentingSystem.Common.EntityValidationConstants.HouseConstants;

public class House
{
    public House()
    {
        this.Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(AddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(DesciptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    [StringLength(ImgURLMaxLength)]
    public string ImageUrl { get; set; } = null!;

    public decimal PricePerMonth { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }

    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Agent))]
    public Guid AgentId { get; set; }
    public virtual Agent Agent { get; set; } = null!;

    [ForeignKey(nameof(Renter))]
    public Guid? RenterId { get; set; }
    public virtual ApplicationUser? Renter { get; set; }
}