using MediatR;
using System.Collections.Generic;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQuery : IRequest<List<TimelineListDto>>
    {
    }
}