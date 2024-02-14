namespace HouseRentingSystem.Web.ViewModels.House;

using System.ComponentModel.DataAnnotations;

using AutoMapper;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Mapping;
using HouseRentingSystem.Web.ViewModels.Category;

using static HouseRentingSystem.Common.EntityValidationConstants.HouseConstants;

public class HouseFormModel : IMapTo<House>, IHaveCustomMappings
{
    public HouseFormModel()
    {
        this.Categories = new HashSet<HouseCategoryFormModel>();
    }
    [Required]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(DesciptionMaxLength, MinimumLength = DesciptionMinLength)]
    public string Description { get; set; } = null!;

    [Required]
    [Display(Name = "Image URL")]
    [StringLength(ImgURLMaxLength)]
    public string ImageUrl { get; set; } = null!;

    [Range(typeof(decimal), PricePerMonthMinRange, PricePerMonthMaxRange)]
    [Display(Name = "Price Per Month")]
    public decimal PricePerMonth { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    public ICollection<HouseCategoryFormModel> Categories { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<HouseFormModel, House>()
            .ForMember(m => m.AgentId, opt => opt.Ignore());
    }
}
