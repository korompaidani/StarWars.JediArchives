namespace StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline
{
    public class UpdateTimelineCommandHandler : AbstractHandler<IEnumerable<Timeline>>, IRequestHandler<UpdateTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTimelineCommandHandler> _logger;
        protected override object CacheKey => typeof(IEnumerable<Timeline>);
        public UpdateTimelineCommandHandler(ITimelineRepository timelineRepository, IMapper mapper, IMemoryCache cache, ILogger<UpdateTimelineCommandHandler> logger) : base(cache)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(UpdateTimelineCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request);
            var timeline = await GetExistingAsync(request.TimelineId);
            _mapper.Map(request, timeline, typeof(UpdateTimelineCommand), typeof(Timeline));
            await _timelineRepository.UpdateAsync(timeline);
            RemoveFromCache(CacheKey);
        }

        private async Task<Timeline> GetExistingAsync(Guid request)
        {
            // todo: can be moved to a common base class
            var timeline = await _timelineRepository.GetByIdAsync(request);

            if (timeline == null)
            {
                _logger.LogWarning($"NotFound Exception was thrown: {nameof(Timeline)} regarding to following request id: {request}");
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
                _logger.LogError($"Validation error in: {nameof(ValidateRequestAsync)} regarding to following request: {request}");
                throw new ValidationException(validationResult);
            }
        }
    }
}