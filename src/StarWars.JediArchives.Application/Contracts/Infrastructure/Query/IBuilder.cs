namespace StarWars.JediArchives.Application.Contracts.Infrastructure.Query
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
