namespace HouseRentingSystem.Web.ViewModels.House;

using AutoMapper;

using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Mapping;


public class RentedHousesViewModel : IMapFrom<House>, IHaveCustomMappings
{
    public string HouseTitle { get; set; } = null!;

    public string HouseImageUrl { get; set; } = null!;

    public string AgentFullName { get; set; } = null!;

    public string AgentEmail { get; set; } = null!;
    
    public string RenterFullName { get; set; } = null !;

    public string RenterEmail { get; set; } = null !;

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<House, RentedHousesViewModel>()
            .ForMember(x => x.HouseTitle, opt => opt.MapFrom(s => s.Title))
            .ForMember(x => x.HouseImageUrl, opt => opt.MapFrom(s => s.ImageUrl))
            .ForMember(d => d.AgentFullName, opt => opt.MapFrom(s => $"{s.Agent.User.FirstName} {s.Agent.User.LastName}"))
            .ForMember(d => d.AgentEmail, opt => opt.MapFrom(s => s.Agent.User.Email))
            .ForMember(d => d.RenterFullName, opt => opt.MapFrom(s => $"{s.Renter!.FirstName} {s.Renter.LastName}"))
            .ForMember(d => d.RenterEmail, opt => opt.MapFrom(s => s.Renter!.Email));
    }
}
