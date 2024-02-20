namespace HouseRentingSystem.Services.Tests;

using Microsoft.EntityFrameworkCore;

using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;

using static DataBaseSeeder;


public class TeAgentServiceTestssts
{
    private DbContextOptions<HouseRentingSystemDbContext> dbOptions;
    private HouseRentingSystemDbContext DbContext;

    private IAgentService agentService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.dbOptions = new DbContextOptionsBuilder<HouseRentingSystemDbContext>()
            .UseInMemoryDatabase("HouseRentingInMemory" + Guid.NewGuid().ToString())
            .Options;

        this.DbContext = new HouseRentingSystemDbContext(this.dbOptions);
        this.DbContext.Database.EnsureCreated();

        SeedDatabase(this.DbContext);

        this.agentService = new AgentService(this.DbContext);
    }

    [Test]
    public async Task AgentExistsByUserIdAsyncShouldReturnTrueWhenExists()
    {
        string existingAgentUserId = AgentUser.Id.ToString();

        bool result = await this.agentService.AgentExistsByUserIdAsync(existingAgentUserId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AgentExistsByUserIdAsyncShouldReturnFalseWhenNotExists()
    {
        string existingAgentUserId = RenterUser.Id.ToString();

        bool result = await this.agentService.AgentExistsByUserIdAsync(existingAgentUserId);

        Assert.That(result, Is.False);
    }
}