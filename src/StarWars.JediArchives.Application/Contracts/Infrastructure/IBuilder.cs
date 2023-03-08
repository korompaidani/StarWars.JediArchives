namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    public interface IBuilder<T>
    {
        T Build<T>() where T : IQueryParser;
    }
}
