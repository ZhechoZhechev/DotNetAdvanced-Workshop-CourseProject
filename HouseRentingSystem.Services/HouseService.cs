namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using System.Collections.Generic;

using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Mapping;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.Agent;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Web.ViewModels.House.Enums;
using HouseRentingSystem.Web.ViewModels.House.ServiceModels;

public class HouseService : IHouseService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public HouseService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Search engine by search term and category
    /// </summary>
    /// <param name="queryModel">query model</param>
    /// <returns></returns>
    public async Task<HousesQueryServiceModel> AllAsync(AllHousesQueryModel queryModel)
    {
        IQueryable<House> housesQuery = dbContext.Houses
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryModel.Category))
        {
            housesQuery = housesQuery
                .Where(h => h.Category.Name == queryModel.Category);
        }

        if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
        {
            var wildCard = $"%{queryModel.SearchString.ToLower()}%";

            housesQuery = housesQuery
                .Where(h =>
                EF.Functions.Like(h.Title, wildCard) ||
                EF.Functions.Like(h.Address, wildCard) ||
                EF.Functions.Like(h.Description, wildCard));
        }

        housesQuery = queryModel.HouseSorting switch
        {
            HouseSorting.Newest => housesQuery.OrderByDescending(h => h.CreatedOn),
            HouseSorting.Oldest => housesQuery.OrderBy(h => h.CreatedOn),
            HouseSorting.PriceAscending => housesQuery.OrderBy(h => h.PricePerMonth),
            HouseSorting.PriceDescending => housesQuery.OrderByDescending(h => h.PricePerMonth),
            _ => housesQuery.OrderBy(h => h.RenterId != null)
        };

        var allHousesModel = await housesQuery
            .Where(h => h.IsActive)
            .Skip((queryModel.CurrentPage - 1) * queryModel.HousesPerPage)
            .Take(queryModel.HousesPerPage)
            .Select(h => new AllHousesViewModel
            {
                Id = h.Id.ToString(),
                Title = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                IsRented = h.RenterId.HasValue
            })
            .ToListAsync();

        var totalHouses = housesQuery.Count();

        return new HousesQueryServiceModel()
        {
            TotalHousesCount = totalHouses,
            Houses = allHousesModel
        };
    }

    /// <summary>
    /// returns all houeses managed by a certain agent
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<AllHousesViewModel>> AllAgentHousesByUserId(string userId)
    {
        var agent = await dbContext.Agents
            .Include(h => h.ManagedHouses)
            .FirstOrDefaultAsync(a => a.UserId.ToString() == userId);
        if (agent == null)
        {
            return null;
        }

        var allHousesModel = agent.ManagedHouses
            .Where(h => h.IsActive)
            .Select(h => new AllHousesViewModel()
            {
                Id = h.Id.ToString(),
                Title = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                IsRented = h.RenterId.HasValue
            })
            .ToList();

        return allHousesModel;
    }

    /// <summary>
    /// returns all houses rented by certain user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<AllHousesViewModel>> AllHousesByUserIdAsync(string userId)
    {
        var user = await dbContext.Users
            .Include(h => h.RentedHouses)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null)
        {
            return null;
        }

        var allRentedHousesForUser = user.RentedHouses
            .Where(h => h.IsActive)
            .Select(h => new AllHousesViewModel()
            {
                Id = h.Id.ToString(),
                Title = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                IsRented = h.RenterId.HasValue
            })
            .ToList();
        return allRentedHousesForUser;
    }
    /// <summary>
    /// creates a house and returns the ID of house created
    /// </summary>
    /// <param name="model"></param>
    /// <param name="agentId"></param>
    /// <returns></returns>
    public async Task<string> CreateHouseAndReturnHouseIdAsync(HouseFormModel model, string agentId)
    {

        var house = AutoMapperConfig.MapperInstance.Map<House>(model);
        house.IsActive = true;
        house.AgentId = Guid.Parse(agentId);

        await dbContext.Houses.AddAsync(house);
        await dbContext.SaveChangesAsync();

        return house.Id.ToString();
    }

    /// <summary>
    /// turns house prop active to false(not deleting if from DB)
    /// </summary>
    /// <param name="houseId"></param>
    /// <returns></returns>
    public async Task DeleteHouseByIdAsync(string houseId)
    {
        var houseToSoftDelete = await dbContext.Houses
            .FirstAsync(h => h.Id.ToString() == houseId);
        houseToSoftDelete.IsActive = false;

        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// edits a house by ID
    /// </summary>
    /// <param name="houseId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task EditHouseByIdAsync(string houseId, HouseFormModel model)
    {
        var house = await dbContext.Houses
            .Where(h => h.IsActive)
            .FirstAsync(h => h.Id.ToString() == houseId);

        house.Title = model.Title;
        house.Address = model.Address;
        house.Description = model.Description;
        house.ImageUrl = model.ImageUrl;
        house.PricePerMonth = model.PricePerMonth;
        house.CategoryId = model.CategoryId;

        await this.dbContext.SaveChangesAsync();
    }
    /// <summary>
    /// returns the house entity that is to be deleted
    /// </summary>
    /// <param name="houseId"></param>
    /// <returns></returns>
    public async Task<HouseDeleteViewModel> GetHouseForDeletionAsync(string houseId)
    {
        var houseToDelete = await dbContext.Houses
            .Where(h => h.IsActive && h.Id.ToString() == houseId)
            .To<HouseDeleteViewModel>()
            .FirstAsync();

        return houseToDelete;
    }
    /// <summary>
    /// retunrs the house entity that is to be edited
    /// </summary>
    /// <param name="houseId"></param>
    /// <returns></returns>
    public Task<HouseFormModel> GetHouseForEditAsync(string houseId)
    {
        var editModel = dbContext.Houses
            .Where(h => h.Id.ToString() == houseId)
            .Select(h => new HouseFormModel()
            {
                Title = h.Title,
                Address = h.Address,
                Description = h.Description,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                CategoryId = h.CategoryId,
            })
            .FirstOrDefaultAsync();

        return editModel!;
    }
    /// <summary>
    /// returns house details by provided ID
    /// </summary>
    /// <param name="houseId"></param>
    /// <returns></returns>
    public Task<HouseDetailsViewModel> HouseDetailsByIdAsync(string houseId)
    {
        var houseModel = dbContext.Houses
            .Where(x => x.Id.ToString() == houseId)
            .Select(h => new HouseDetailsViewModel()
            {
                Id = h.Id.ToString(),
                Title = h.Title,
                Address = h.Address,
                Description = h.Description,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                Category = h.Category.Name,
                IsRented = h.RenterId.HasValue,
                Agent = new AgentInfoViewModel()
                {
                    Email = h.Agent.User.Email!,
                    PhoneNumber = h.Agent.PhoneNumber

                }
            })
            .FirstOrDefaultAsync();

        return houseModel;
    }
    /// <summary>
    /// Check if a house exists by given ID
    /// </summary>
    /// <param name="houseId"></param>
    /// <returns></returns>
    public async Task<bool> HouseExistsByIdAsync(string houseId)
    {
        return await dbContext.Houses
            .Where(h => h.IsActive)
            .AnyAsync(h => h.Id.ToString() == houseId);
    }
    /// <summary>
    /// Check if agent with given ID owns a house with certain ID
    /// </summary>
    /// <param name="houseId"></param>
    /// <param name="agentId"></param>
    /// <returns></returns>
    public async Task<bool> IsAgentWithIdOwnerHouseWithIdAsync(string houseId, string agentId)
    {
        var house = await dbContext.Houses
            .Where(h => h.IsActive)
            .FirstOrDefaultAsync(h => h.Id.ToString() == houseId);

        if (house.AgentId.ToString() == agentId) return true;
        else return false;
    }
    /// <summary>
    /// Returns a bool for if the house is rented for a certain ID
    /// </summary>
    /// <param name="houseId"></param>
    /// <returns></returns>
    public async Task<bool> IsHouseRentedAsync(string houseId)
    {
        return await dbContext.Houses
            .Where(h => h.IsActive && h.Id.ToString() == houseId)
            .Select(h => h.RenterId.HasValue)
            .FirstAsync();

    }
    /// <summary>
    /// Returns a bool for if a certain user is a rentier of a certain house
    /// </summary>
    /// <param name="houseId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> IsUserHouseRentierAsync(string houseId, string userId)
    {
        var house = await dbContext.Houses
            .FirstAsync(h => h.Id.ToString() == houseId);

        return house.RenterId.ToString() == userId;
    }
    public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
    {
        var lastThreeHouses = await this.dbContext.Houses
            .Where(h => h.IsActive)
            .OrderByDescending(h => h.CreatedOn)
            .Take(3)
            .To<IndexViewModel>()
            .ToArrayAsync();

        return lastThreeHouses;
    }

    public async Task LeaveHouse(string houseId)
    {
        var house = await dbContext.Houses
           .Where(h => h.IsActive)
           .FirstAsync(h => h.Id.ToString() == houseId);

        house.RenterId = null;
        await dbContext.SaveChangesAsync();
    }

    public async Task RentHouse(string houseId, string userId)
    {
        var house = await dbContext.Houses
            .Where(h => h.IsActive)
            .FirstAsync(h => h.Id.ToString() == houseId);

        house.RenterId = Guid.Parse(userId);

        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<RentedHousesViewModel>> AllRentedHousesAsync()
    {
        var rentedHousesModel = await dbContext.Houses
            .Include(h => h.Agent)
            .Include(h => h.Agent.User)
            .Include(h => h.Renter)
            .Where(h => h.IsActive && h.RenterId.HasValue)
            .To<RentedHousesViewModel>()
            .ToArrayAsync();

        return rentedHousesModel;
    }
}
