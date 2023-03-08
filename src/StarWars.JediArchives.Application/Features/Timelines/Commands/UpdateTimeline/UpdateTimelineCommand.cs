namespace StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline
{
    public class UpdateTimelineCommand : IRequest
    {
        public Guid TimelineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }

        public override string ToString()
        {
            return $"{nameof(TimelineId)}: {TimelineId}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(StartYear)}: {StartYear}, {nameof(EndYear)}: {EndYear}, {nameof(ImageUrl)}: {ImageUrl}, {nameof(CategoryId)}: {CategoryId}";
        }
    }
}
