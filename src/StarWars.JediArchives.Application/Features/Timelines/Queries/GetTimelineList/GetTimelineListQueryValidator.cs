using FluentValidation;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryValidator : AbstractValidator<GetTimelineListQuery>
    {
        public GetTimelineListQueryValidator()
        {
            RuleFor(p => p.Page)
                .GreaterThan(0);

            RuleFor(p => p.Size)
                .GreaterThan(0);
        }
    }
}