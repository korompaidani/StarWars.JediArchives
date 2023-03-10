namespace StarWars.JediArchives.Application.Contracts.Infrastructure
{
    /// <summary>
    /// This is the highest layer of incomping complex Query handling
    /// </summary>
    public interface IQueryParserService
    {
        ComplexParameters Validate(Type targetType);
    }
}
