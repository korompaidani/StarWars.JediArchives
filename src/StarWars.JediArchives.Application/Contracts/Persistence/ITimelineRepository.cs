namespace StarWars.JediArchives.Application.Contracts.Persistence
{
    public interface ITimelineRepository : IAsyncRepository<Timeline>
    {
        Task<bool> IsTimelineNameUniqueAsync(string name);
    }
}
