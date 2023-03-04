using StarWars.DataTank.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<List<Category>> GetCategoriesWithTimeLines();
    }
}
