﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Application.Exceptions;
using StarWars.JediArchives.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryHandler : IRequestHandler<GetTimelineListQuery, List<TimelineListDto>>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTimelineListQueryHandler> _logger;

        public GetTimelineListQueryHandler(ITimelineRepository timelineRepository, IMapper mapper, ILogger<GetTimelineListQueryHandler> logger)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TimelineListDto>> Handle(GetTimelineListQuery request, CancellationToken cancellationToken)
        {            
            IEnumerable<Timeline> timelineList;

            if (request.Page is null || request.Size is null)
            {
                timelineList = (await _timelineRepository.ListAllAsync());
            }
            else
            {
                await ValidateRequestAsync(request);
                timelineList = await _timelineRepository.GetPagedReponseAsync(request.Page.Value, request.Size.Value);
            }

            return _mapper.Map<List<TimelineListDto>>(timelineList);
        }

        private async Task ValidateRequestAsync(GetTimelineListQuery request)
        {
            var validator = new GetTimelineListQueryValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                _logger.LogError($"Validation error in: {nameof(ValidateRequestAsync)} regarding to following request: {request}");
                throw new ValidationException(validationResult);
            }
        }
    }
}