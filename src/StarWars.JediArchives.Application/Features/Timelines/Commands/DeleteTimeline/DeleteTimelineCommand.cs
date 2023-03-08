namespace StarWars.JediArchives.Application.Features.Timelines.Commands.DeleteTimeline
{
    public class DeleteTimelineCommand : IRequest
    {
        public Guid TimelineId { get; set; }
    }
}
