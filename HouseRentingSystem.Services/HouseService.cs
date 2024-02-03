namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using System.Collections.Generic;

using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
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
            HouseSorting.Newest => dbContext.Houses.OrderBy(h => h.CreatedOn),
            HouseSorting.Oldest => dbContext.Houses.OrderByDescending(h => h.CreatedOn),
            HouseSorting.PriceAscending => dbContext.Houses.OrderBy(h => h.PricePerMonth),
            HouseSorting.PriceDescending => dbContext.Houses.OrderByDescending(h => h.PricePerMonth),
            HouseSorting.NotRentedFirst => dbContext.Houses.OrderBy(h => h.RenterId != null),
        };

        var allHousesModel = await housesQuery
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
            AgentId = Guid.Parse(agentId)
        };

        await dbContext.Houses.AddAsync(house);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
    {
        var lastThreeHouses = await this.dbContext.Houses
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
