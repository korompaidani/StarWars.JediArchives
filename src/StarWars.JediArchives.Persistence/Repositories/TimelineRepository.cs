namespace StarWars.JediArchives.Persistence.Repositories
{
    public class TimelineRepository : BaseRepository<Timeline>, ITimelineRepository
    {
        public TimelineRepository(StarWarsJediArchivesDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsTimelineNameUniqueAsync(string name)
        {
            var matches = await _dbContext.TimeLines.AnyAsync(e => e.Name.Equals(name));
            return matches;
        }
    }
}
