namespace HouseRentingSystem.Web.ViewModels.House;

using System.ComponentModel.DataAnnotations;

using HouseRentingSystem.Web.ViewModels.House.Enums;
using static HouseRentingSystem.Common.GeneralApplicationConstants;

public class AllHousesQueryModel
{
    public AllHousesQueryModel()
    {
        this.CurrentPage = CurrentPageConst;
        this.HousesPerPage = HousesPerPageConst;

        this.Categories = new HashSet<string>();
        this.Houses = new HashSet<AllHousesViewModel>();
    }

    public string? Category { get; set; }

    [Display(Name = "Search By Word")]
    public string? SearchString { get; set;}

    [Display(Name = "Sort Houses By")]
    public HouseSorting HouseSorting { get; set; }

    public int CurrentPage { get; set; }

    public int HousesPerPage { get; set; }

    public int TotalHouses { get; set; }

    public IEnumerable<string> Categories { get; set; }

    public IEnumerable<AllHousesViewModel> Houses { get; set; }
}
