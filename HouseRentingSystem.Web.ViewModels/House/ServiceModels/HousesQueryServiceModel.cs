namespace HouseRentingSystem.Web.ViewModels.House.ServiceModels;

public class HousesQueryServiceModel
{
    public HousesQueryServiceModel()
    {
        this.Houses = new HashSet<AllHousesViewModel>();
    }

    public int TotalHousesCount { get; set; }

    public IEnumerable<AllHousesViewModel> Houses {  get; set; }
}
