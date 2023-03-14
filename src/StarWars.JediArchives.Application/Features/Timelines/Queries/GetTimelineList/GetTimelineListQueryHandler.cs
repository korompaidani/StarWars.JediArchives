namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryHandler : AbstractHandler<IEnumerable<Timeline>>, IRequestHandler<GetTimelineListQuery, PagedList<TimelineListDto>>
    {
        private const string TimelineListCacheKey = "timelineList";

        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;
        private readonly IQueryProcessor<TimelineListDto> _queryProcessor;
        private readonly ILogger<GetTimelineListQueryHandler> _logger;

        public GetTimelineListQueryHandler(ITimelineRepository timelineRepository, IMapper mapper, IQueryProcessor<TimelineListDto> queryProcessor, IMemoryCache cache, ILogger<GetTimelineListQueryHandler> logger) : base(cache)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
            _queryProcessor = queryProcessor;
            _logger = logger;
        }

        public async Task<PagedList<TimelineListDto>> Handle(GetTimelineListQuery request, CancellationToken cancellationToken)
        {
            ProcessQueries(request);

            IEnumerable<Timeline> timeListFromRepository = null;

            if (TryGetFromCache(TimelineListCacheKey, out timeListFromRepository))
            {
                _logger.LogInformation($"{nameof(GetTimelineListQueryHandler)}.{nameof(Handle)}: {nameof(IEnumerable<Timeline>)} entries was found in cache.");
            }
            else
            {
                timeListFromRepository = await _timelineRepository.ListAllAsync();
                _logger.LogInformation($"{nameof(GetTimelineListQueryHandler)}.{nameof(Handle)}: {nameof(IEnumerable<Timeline>)} entries was not found in cache. Data are requested from the db.");

                AddToCache(TimelineListCacheKey, timeListFromRepository);
            }

            bool isIgnored = false;
            CheckPaginationWasRequestedByUser(request, ref isIgnored);

            timeListFromRepository = PagedList<Timeline>.ToPagedList((await _timelineRepository.ListAllAsync()), request.PageNumber, request.PageSize, isIgnored);

            return _mapper.Map<PagedList<TimelineListDto>>(timeListFromRepository);
        }

        private void CheckPaginationWasRequestedByUser(GetTimelineListQuery request, ref bool isIgnored)
        {
            if (!request.IsPagingCustomized)
            {
                isIgnored = true;
                _logger.LogInformation($"{nameof(GetTimelineListQueryHandler)}.{nameof(Handle)}: There was no pagination in request. All entry is returning from db.");
            }
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