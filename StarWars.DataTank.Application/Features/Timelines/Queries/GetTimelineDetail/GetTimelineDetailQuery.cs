using MediatR;
using System;

namespace StarWars.DataTank.Application.Features.Timelines.Queries.GetTimelineDetail
{
    public class GetTimelineDetailQuery : IRequest<TimelineDetailDto>
    {
        public Guid TimelineId { get; set; }
    }
}