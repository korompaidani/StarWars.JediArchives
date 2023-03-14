namespace StarWars.JediArchives.Application.Features.Timelines.Commands.CreateTimeline
{
    public class CreateTimelineCommandHandler : AbstractHandler<IEnumerable<Timeline>>, IRequestHandler<CreateTimelineCommand, Guid>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTimelineCommandHandler> _logger;

        protected override object CacheKey => typeof(IEnumerable<Timeline>);

        public CreateTimelineCommandHandler(ITimelineRepository timelineRepository, IMapper mapper, IMemoryCache cache, ILogger<CreateTimelineCommandHandler> logger) : base(cache)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTimelineCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request);

            var timeline = _mapper.Map<Timeline>(request);
            var addedTimeline = await _timelineRepository.AddAsync(timeline);
            RemoveFromCache(CacheKey);

            return addedTimeline.TimelineId;
        }

        private async Task ValidateRequestAsync(CreateTimelineCommand request)
        {
            // todo: dependency incejtion instead
            var validator = new CreateTimelineCommandValidator(_timelineRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                _logger.LogError($"Validation error in: {nameof(ValidateRequestAsync)} regarding to following request: {request}");
                throw new ValidationException(validationResult);
            }
        }
    }
}