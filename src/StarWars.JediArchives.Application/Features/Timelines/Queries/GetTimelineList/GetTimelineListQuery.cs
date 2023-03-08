namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQuery : IParametrizedDto, IRequest<List<TimelineListDto>>
    {
        public int? Count { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
        public string Query { get; set; }
    }
}