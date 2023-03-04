using MediatR;
using System.Collections.Generic;

namespace StarWars.DataTank.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQuery : IRequest<List<TimelineListDto>>
    {
    }
}