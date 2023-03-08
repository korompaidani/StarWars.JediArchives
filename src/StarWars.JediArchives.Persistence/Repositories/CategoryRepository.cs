

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