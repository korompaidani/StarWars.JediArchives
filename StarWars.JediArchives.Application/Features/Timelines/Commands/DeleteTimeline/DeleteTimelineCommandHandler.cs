using MediatR;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Application.Exceptions;
using StarWars.JediArchives.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Features.Timelines.Commands.DeleteTimeline
{
    public class DeleteTimelineCommandHandler : IRequestHandler<DeleteTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;

        public DeleteTimelineCommandHandler(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;
        }

        public async Task Handle(DeleteTimelineCommand request, CancellationToken cancellationToken)
        {
            var timelineTobeDeleted = await GetExistingAsync(request.TimelineId);
            await _timelineRepository.DeleteAsync(timelineTobeDeleted);
        }

        private async Task<Timeline> GetExistingAsync(Guid request)
        {
            // todo: can be moved to a common base class
            var timeline = await _timelineRepository.GetByIdAsync(request);

            if (timeline == null)
            {
                throw new NotFoundException(nameof(Timeline), request);
            }

            return timeline;
        }
    }
}
