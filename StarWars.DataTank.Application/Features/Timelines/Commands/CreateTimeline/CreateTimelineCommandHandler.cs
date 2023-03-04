using AutoMapper;
using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.CreateTimeline
{
    public class CreateTimelineCommandHandler : IRequestHandler<CreateTimelineCommand, Guid>
    {
        private readonly IAsyncRepository<Timeline> _timelineRepository;
        private readonly IMapper _mapper;

        public CreateTimelineCommandHandler(IAsyncRepository<Timeline> timelineRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
        }

        async public Task<Guid> Handle(CreateTimelineCommand request, CancellationToken cancellationToken)
        {
            var timeline = _mapper.Map<Timeline>(request);
            var addedTimeline = await _timelineRepository.AddAsync(timeline);

            return addedTimeline.TimelineId;
        }
    }
}