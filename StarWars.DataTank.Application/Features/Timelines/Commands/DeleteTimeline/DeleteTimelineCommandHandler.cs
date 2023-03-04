using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Application.Exceptions;
using StarWars.DataTank.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.DeleteTimeline
{
    public class DeleteTimelineCommandHandler : IRequestHandler<DeleteTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;

        public DeleteTimelineCommandHandler(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;
        }

        async public Task<Unit> Handle(DeleteTimelineCommand request, CancellationToken cancellationToken)
        {
            var timelineTobeDeleted = await _timelineRepository.GetByIdAsync(request.TimelineId);
            
            if (timelineTobeDeleted == null)
            {
                throw new NotFoundException(nameof(Timeline), request.TimelineId);
            }

            await _timelineRepository.DeleteAsync(timelineTobeDeleted);

            return Unit.Value;
        }
    }
}
