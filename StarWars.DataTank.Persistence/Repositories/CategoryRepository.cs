using Microsoft.EntityFrameworkCore;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.DataTank.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(StarWarsJediArchivesDbContext dbContext) : base(dbContext)
        {
        }

        async public Task<List<Category>> GetCategoriesWithTimeLines()
        {
            var allCategories = await _dbContext.Categories.Include(x => x.Timelines).ToListAsync();
            return allCategories;
        }
    }
}