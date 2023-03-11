namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
