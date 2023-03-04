using AutoMapper;
using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Application.Exceptions;
using StarWars.DataTank.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.CreateTimeline
{
    public class CreateTimelineCommandHandler : IRequestHandler<CreateTimelineCommand, Guid>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;

        public CreateTimelineCommandHandler(ITimelineRepository timelineRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
        }

        async public Task<Guid> Handle(CreateTimelineCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request);

            var timeline = _mapper.Map<Timeline>(request);
            var addedTimeline = await _timelineRepository.AddAsync(timeline);

            return addedTimeline.TimelineId;
        }

        private async Task ValidateRequestAsync(CreateTimelineCommand request)
        {
            // todo: dependency incejtion instead
            var validator = new CreateTimelineCommandValidator(_timelineRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new ValidationException(validationResult);
            }
        }
    }
}