namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail
{
    public record CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
    }
}