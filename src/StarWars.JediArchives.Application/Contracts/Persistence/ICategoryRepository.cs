using StarWars.JediArchives.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<List<Category>> GetCategoriesWithTimeLines();
    }
}
