using Microsoft.EntityFrameworkCore;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Domain.Models;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Persistence.Repositories
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
