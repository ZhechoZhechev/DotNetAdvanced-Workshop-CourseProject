using HouseRentingSystem.Web.ViewModels.Agent;

namespace HouseRentingSystem.Services.Interfaces;

public interface IAgentService
{
    Task<bool> AgentExistsByUserIdAsync(string userId);

    Task<bool> AgentExistsByPhoneNumberAsync(string phoneNumber);

    Task<bool> UserHasRentsAsync(string userId);

    Task CreateAgentAsync(string userId, BecomeAgentFormModel model);

    Task<string?> AgentIdByUserIdAsync(string userId);
}
