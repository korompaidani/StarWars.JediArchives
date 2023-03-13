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

            if (request.PageNumber == 0 || request.PageSize == 0)
            {
                timelineList = (await _timelineRepository.ListAllAsync());
            }
            else
            {
                await ValidateRequestAsync(request);
                timelineList = await _timelineRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize.Value);
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