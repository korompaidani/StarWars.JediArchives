namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryValidator : AbstractValidator<GetTimelineListQuery>
    {
        public GetTimelineListQueryValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThan(0);

            RuleFor(p => p.PageSize)
                .GreaterThan(0);
        }
    }
}