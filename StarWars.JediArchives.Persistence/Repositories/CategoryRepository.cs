using Microsoft.EntityFrameworkCore;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(StarWarsJediArchivesDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Category>> GetCategoriesWithTimeLines()
        {
            var allCategories = await _dbContext.Categories.Include(x => x.Timelines).ToListAsync();
            return allCategories;
        }
    }
}