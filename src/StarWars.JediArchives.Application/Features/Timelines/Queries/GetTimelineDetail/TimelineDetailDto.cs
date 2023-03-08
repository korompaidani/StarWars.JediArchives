namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail
{
    public record TimelineDetailDto
    {
        public Guid TimelineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Length => EndYear - StartYear;
        public string ImageUrl { get; set; }
        public Image Image { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
