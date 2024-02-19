namespace HouseRentingSystem.Web.ViewModels.User;

using AutoMapper;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Mapping;


public class UserViewModel : IMapFrom<Agent>, IMapFrom<ApplicationUser>, IHaveCustomMappings
{
    public string UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Agent, UserViewModel>()
            .ForMember(a => a.Email, opt => opt.MapFrom(s => s.User.Email))
            .ForMember(a => a.FullName, opt => opt.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));

        configuration.CreateMap<ApplicationUser, UserViewModel>()
            .ForMember(a => a.UserId, opt => opt.MapFrom(s => s.Id))
            .ForMember(u => u.PhoneNumber, opt => opt.MapFrom(s => string.Empty))
            .ForMember(u => u.FullName, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));
    }
}
