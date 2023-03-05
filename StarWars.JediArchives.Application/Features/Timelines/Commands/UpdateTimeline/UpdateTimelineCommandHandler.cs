using AutoMapper;
using MediatR;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Application.Exceptions;
using StarWars.JediArchives.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline
{
    public class UpdateTimelineCommandHandler : IRequestHandler<UpdateTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;

        public UpdateTimelineCommandHandler(ITimelineRepository timelineRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateTimelineCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request);
            var timeline = await GetExistingAsync(request.TimelineId);
            _mapper.Map(request, timeline, typeof(UpdateTimelineCommand), typeof(Timeline));
            await _timelineRepository.UpdateAsync(timeline);

            return Unit.Value;
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

        private async Task ValidateRequestAsync(UpdateTimelineCommand request)
        {
            // todo: dependency incejtion instead
            var validator = new UpdateTimelineCommandValidator(_timelineRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new ValidationException(validationResult);
            }
        }
    }
}