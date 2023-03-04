using StarWars.JediArchives.Domain.Models;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Contracts.Persistence
{
    public interface ITimelineRepository : IAsyncRepository<Timeline>
    {
        Task<bool> IsTimelineNameUniqueAsync(string name);
    }
}
