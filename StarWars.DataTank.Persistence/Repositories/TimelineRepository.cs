using Microsoft.EntityFrameworkCore;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Domain.Models;
using System.Threading.Tasks;

namespace StarWars.DataTank.Persistence.Repositories
{
    public class TimelineRepository : BaseRepository<Timeline>, ITimelineRepository
    {
        public TimelineRepository(StarWarsJediArchivesDbContext dbContext) : base(dbContext)
        {
        }

        async public Task<bool> IsTimelineNameUniqueAsync(string name)
        {
            var matches = await _dbContext.TimeLines.AnyAsync(e => e.Name.Equals(name));
            return matches;
        }
    }
}
