namespace HouseRentingSystem.Web.ViewModels.House;

using System.ComponentModel.DataAnnotations;

using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Mapping;

public class HouseDeleteViewModel : IMapFrom<House>
{
    public string Address { get; set; } = null!;

    public string Title { get; set; } = null!;

    [Display(Name = "Image Link")]
    public string ImageUrl { get; set; } = null!;
}
