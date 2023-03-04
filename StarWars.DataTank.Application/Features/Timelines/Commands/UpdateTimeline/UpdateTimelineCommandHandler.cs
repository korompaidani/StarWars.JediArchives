﻿using AutoMapper;
using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Application.Exceptions;
using StarWars.DataTank.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Commands.UpdateTimeline
{
    public class UpdateTimelineCommandHandler : IRequestHandler<UpdateTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;

        public UpdateTimelineCommandHandler(ITimelineRepository timelineRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
        }

        async public Task<Unit> Handle(UpdateTimelineCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request);
            var timeline = await GetExistingAsync(request.TimelineId);
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