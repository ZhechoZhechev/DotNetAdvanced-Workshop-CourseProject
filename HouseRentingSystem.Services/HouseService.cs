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

    public async Task<string> CreateHouseAndReturnHouseIdAsync(HouseFormModel model, string agentId)
    {

        var house = AutoMapperConfig.MapperInstance.Map<House>(model);
        house.IsActive = true;
        house.AgentId = Guid.Parse(agentId);

        await dbContext.Houses.AddAsync(house);
        await dbContext.SaveChangesAsync();

        return house.Id.ToString();
    }

    public async Task DeleteHouseByIdAsync(string houseId)
    {
        var houseToSoftDelete = await dbContext.Houses
            .FirstAsync(h => h.Id.ToString() == houseId);
        houseToSoftDelete.IsActive = false;

        await dbContext.SaveChangesAsync();
    }

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

    public async Task<HouseDeleteViewModel> GetHouseForDeletionAsync(string houseId)
    {
        var houseToDelete = await dbContext.Houses
            .Where(h => h.IsActive && h.Id.ToString() == houseId)
            .To<HouseDeleteViewModel>()
            .FirstAsync();

        return houseToDelete;
    }

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

        return editModel;
    }

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

    public async Task<bool> HouseExistsByIdAsync(string houseId)
    {
        return await dbContext.Houses
            .Where(h => h.IsActive)
            .AnyAsync(h => h.Id.ToString() == houseId);
    }

    public async Task<bool> IsAgentWithIdOwnerHouseWithIdAsync(string houseId, string agentId)
    {
        var house = await dbContext.Houses
            .Where(h => h.IsActive)
            .FirstOrDefaultAsync(h => h.Id.ToString() == houseId);

        if (house.AgentId.ToString() == agentId) return true;
        else return false;
    }

    public async Task<bool> IsHouseRentedAsync(string houseId)
    {
        return await dbContext.Houses
            .Where(h => h.IsActive && h.Id.ToString() == houseId)
            .Select(h => h.RenterId.HasValue)
            .FirstAsync();

    }

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
}
