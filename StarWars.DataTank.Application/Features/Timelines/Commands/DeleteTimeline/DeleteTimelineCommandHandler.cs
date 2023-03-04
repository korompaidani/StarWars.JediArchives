using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
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
            var timelineTobeDeleted = await _asyncRepository.GetByIdAsync(request.TimeLineId);
            
            if (timelineTobeDeleted == null)
            {
                //TODO throw an error in case of not found
            }

            await _asyncRepository.DeleteAsync(timelineTobeDeleted);

            return Unit.Value;
        }
    }
}
