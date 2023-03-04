using AutoMapper;
using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.UpdateTimeline
{
    public class UpdateTimelineCommandHandler : IRequestHandler<UpdateTimelineCommand>
    {
        private readonly IAsyncRepository<Timeline> _timelineRepository;

        public UpdateTimelineCommandHandler(IAsyncRepository<Timeline> timelineRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
        }

        async public Task<Unit> Handle(UpdateTimelineCommand request, CancellationToken cancellationToken)
        {
            var timeline = await _timelineRepository.GetByIdAsync(request.TimelineId);

            if (timeline == null)
            {
                //TODO: Throw an error if not found
            }

            await _timelineRepository.UpdateAsync(timeline);

            return Unit.Value;
        }
    }
}