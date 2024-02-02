namespace HouseRentingSystem.Services;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Category;

public class CategoryService : ICategoryService
{
    private readonly HouseRentingSystemDbContext dbContext;

    public CategoryService(HouseRentingSystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<ICollection<HouseCategoryFormModel>> AllHouseCategoriesAsync()
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .Select(c => new HouseCategoryFormModel()
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToListAsync();

        return categories;
    }
}
