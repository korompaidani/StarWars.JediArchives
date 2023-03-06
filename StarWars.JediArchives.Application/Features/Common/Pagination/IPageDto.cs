namespace StarWars.JediArchives.Application.Features.Common.Dto
{
    public interface IPageDto
    {
        int? Count { get; set; }
        int? Page { get; set; }
        int? Size { get; set; }
    }
}
