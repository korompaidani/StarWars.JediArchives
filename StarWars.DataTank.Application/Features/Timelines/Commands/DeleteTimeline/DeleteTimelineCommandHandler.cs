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
        private readonly IAsyncRepository<Timeline> _asyncRepository;

        public DeleteTimelineCommandHandler(IAsyncRepository<Timeline> asyncRepository)
        {
            _asyncRepository = asyncRepository;
        }

        async public Task<Unit> Handle(DeleteTimelineCommand request, CancellationToken cancellationToken)
        {
            var timelineTobeDeleted = await _asyncRepository.GetByIdAsync(request.TimelineId);
            
            if (timelineTobeDeleted == null)
            {
                throw new NotFoundException(nameof(Timeline), request.TimelineId);
            }

            await _asyncRepository.DeleteAsync(timelineTobeDeleted);

            return Unit.Value;
        }
    }
}
