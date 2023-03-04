using MediatR;
using System;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail
{
    public class GetTimelineDetailQuery : IRequest<TimelineDetailDto>
    {
        public Guid TimelineId { get; set; }
    }
}