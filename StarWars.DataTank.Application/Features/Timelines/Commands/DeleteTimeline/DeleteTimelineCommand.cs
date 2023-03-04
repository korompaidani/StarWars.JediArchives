using MediatR;
using System;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.DeleteTimeline
{
    public class DeleteTimelineCommand : IRequest
    {
        public Guid TimelineId { get; set; }
    }
}
