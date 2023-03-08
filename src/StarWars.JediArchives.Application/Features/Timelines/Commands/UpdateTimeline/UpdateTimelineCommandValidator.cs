namespace StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline
{
    public class UpdateTimelineCommandValidator : AbstractValidator<UpdateTimelineCommand>
    {
        private readonly ITimelineRepository _timelineRepository;

        public UpdateTimelineCommandValidator(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;
        }

        private async Task<bool> TimelineNameIsUnique(UpdateTimelineCommand e, CancellationToken token)
        {
            return !(await _timelineRepository.IsTimelineNameUniqueAsync(e.Name));
        }
    }
}