namespace StarWars.JediArchives.Application.Features.Common.Dto
{
    public interface IParametrizedDto
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string QueryString { get; set; }
    }
}