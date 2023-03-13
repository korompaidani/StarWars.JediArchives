namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryHandler : IRequestHandler<GetTimelineListQuery, PagedList<TimelineListDto>>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;
        private readonly IQueryProcessor<TimelineListDto> _queryProcessor;
        private readonly ILogger<GetTimelineListQueryHandler> _logger;

        public GetTimelineListQueryHandler(ITimelineRepository timelineRepository, IMapper mapper, IQueryProcessor<TimelineListDto> queryProcessor, ILogger<GetTimelineListQueryHandler> logger)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
            _queryProcessor = queryProcessor;
            _logger = logger;
        }

        public async Task<PagedList<TimelineListDto>> Handle(GetTimelineListQuery request, CancellationToken cancellationToken)
        {
            ProcessQueries(request);

            IEnumerable<Timeline> timelineList = null;

            if (request.PageNumber != 0 && request.PageSize != 0)
            {
                timelineList = PagedList<Timeline>.ToPagedList((await _timelineRepository.ListAllAsync()), request.PageNumber, request.PageSize);
            }
            else
            {
                await ValidateRequestAsync(request);
                timelineList = PagedList<Timeline>.ToPagedList((await _timelineRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize)), request.PageNumber, request.PageSize);
            }
            return _mapper.Map<PagedList<TimelineListDto>>(timelineList);
        }

        // TODO: transform to async the QueryProcessor internally
        private void ProcessQueries(GetTimelineListQuery request)
        {
            if (!string.IsNullOrEmpty(request.QueryString))
            {
                _queryProcessor.Run(request.QueryString);
            }
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