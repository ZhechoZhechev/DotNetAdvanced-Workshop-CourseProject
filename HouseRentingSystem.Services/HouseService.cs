namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using System.Collections.Generic;

using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
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

    public async Task<ICollection<AllHousesViewModel>> AllHousesByAgentIdAsync(string userId)
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

    public async Task<ICollection<AllHousesViewModel>> AllHousesByUserIdAsync(string userId)
    {
        var user = await dbContext.Users
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

    public async Task CreateHouse(HouseFormModel model, string agentId)
    {
        var house = new House()
        {
            Title = model.Title,
            Address = model.Address,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            PricePerMonth = model.PricePerMonth,
            CategoryId = model.CategoryId,
            IsActive = true,
            AgentId = Guid.Parse(agentId)
        };

        await dbContext.Houses.AddAsync(house);
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

    public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
    {
        var lastThreeHouses = await this.dbContext.Houses
            .Where(h => h.IsActive)
            .OrderByDescending(h => h.CreatedOn)
            .Take(3)
            .Select(h => new IndexViewModel
            {
                Id = h.Id.ToString(),
                Title = h.Title,
                ImageUrl = h.ImageUrl
            })
            .ToArrayAsync();

        return lastThreeHouses;
    }
}
