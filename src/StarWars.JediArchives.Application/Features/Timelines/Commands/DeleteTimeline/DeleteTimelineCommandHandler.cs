namespace StarWars.JediArchives.Application.Features.Timelines.Commands.DeleteTimeline
{
    public class DeleteTimelineCommandHandler : AbstractHandler<IEnumerable<Timeline>>, IRequestHandler<DeleteTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly ILogger<UpdateTimelineCommandHandler> _logger;

        protected override object CacheKey => typeof(IEnumerable<Timeline>);
        public DeleteTimelineCommandHandler(ITimelineRepository timelineRepository, IMemoryCache cache, ILogger<UpdateTimelineCommandHandler> logger) : base(cache)
        {
            _timelineRepository = timelineRepository;
            _logger = logger;
        }

        public async Task Handle(DeleteTimelineCommand request, CancellationToken cancellationToken)
        {
            var timelineTobeDeleted = await GetExistingAsync(request.TimelineId);
            await _timelineRepository.DeleteAsync(timelineTobeDeleted);
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
    }
}