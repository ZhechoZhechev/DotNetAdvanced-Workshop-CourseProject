using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Agent;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services;

public class AgentService : IAgentService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public AgentService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> AgentExistsByPhoneNumberAsync(string phoneNumber)
    {
        return await dbContext.Agents
            .AnyAsync(a => a.PhoneNumber == phoneNumber);
    }

    public async Task<bool> AgentExistsByUserIdAsync(string userId)
    {
        return await dbContext.Agents
            .AnyAsync(a => a.UserId.ToString() == userId);
    }

    public async Task<string?> AgentIdByUserIdAsync(string userId)
    {
        var agent = await dbContext.Agents
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

        if (agent == null) return null;

        return agent.Id.ToString();    
    }

    public async Task CreateAgentAsync(string userId, BecomeAgentFormModel model)
    {
        var newAgent = new Agent
        {
            UserId = Guid.Parse(userId),
            PhoneNumber = model.PhoneNumber
        };

        await dbContext.Agents.AddAsync(newAgent);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UserHasRentsAsync(string userId)
    {
        return await dbContext.Houses
            .AnyAsync(u => u.RenterId.ToString() == userId);
    }
}
