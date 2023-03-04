using StarWars.DataTank.Domain.Models;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Contracts.Persistence
{
    public interface ITimelineRepository : IAsyncRepository<Timeline>
    {
        Task<bool> IsTimelineNameUnique(string name);
    }
}
