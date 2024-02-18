namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.User;
using HouseRentingSystem.Services.Mapping;

public class UserService : IUserService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public UserService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<UserViewModel>> AllUsersAsync()
    {
        var allUsers = new List<UserViewModel>();

        var agents = await dbContext.Agents
            .Include(u => u.User)
            .To<UserViewModel>()
            .ToArrayAsync();
        allUsers.AddRange(agents);

        var users = await dbContext.Users
            .Where(u => !dbContext.Agents.Any(a => a.UserId == u.Id))
            .To<UserViewModel>()
            .ToArrayAsync();
        allUsers.AddRange(users);

        return allUsers;
    }

    public async Task<string> GetFullNameByUserIdAsync(string userId)
    {
        var fullName = await dbContext.Users
            .Where(u => u.Id.ToString() == userId)
            .Select(u => $"{u.FirstName} {u.LastName}")
            .FirstOrDefaultAsync();

        if (fullName == null)
        {
            return "No such user!";
        }

        return fullName;
    }
}
