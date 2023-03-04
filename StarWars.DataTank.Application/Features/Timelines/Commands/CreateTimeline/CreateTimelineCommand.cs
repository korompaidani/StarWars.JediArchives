using MediatR;
using System;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.CreateTimeline
{
    public class CreateTimelineCommand : IRequest<Guid>
    {
        public Guid TimelineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
    }
}
