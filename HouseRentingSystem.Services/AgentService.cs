using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services;

public class AgentService : IAgentService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public AgentService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> AgentExistsByUserId(string userId)
    {
        return await dbContext.Agents
            .AnyAsync(a => a.UserId.ToString() == userId);
    }
}
