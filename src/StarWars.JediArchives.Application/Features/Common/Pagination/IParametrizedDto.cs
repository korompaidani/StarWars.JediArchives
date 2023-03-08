namespace StarWars.JediArchives.Application.Features.Common.Dto
{
    public interface IParametrizedDto
    {
        int? Count { get; set; }
        int? Page { get; set; }
        int? Size { get; set; }
        string Query { get; set; }
    }
}