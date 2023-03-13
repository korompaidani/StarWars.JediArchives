namespace StarWars.JediArchives.Api.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQueryProcessor<TimelineListDto> _queryProcessor;

        public TimelineController(IMediator mediator, IQueryProcessor<TimelineListDto> queryProcessor)
        {
            _mediator = mediator;
            _queryProcessor = queryProcessor;
        }

        [HttpGet("all", Name = "GetAllTimelines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<TimelineListDto>>> GetAllTimelines([FromQuery] GetTimelineListQuery timelineListQuery)
        {
            //_queryProcessor.Run(query);

            var dtos = await _mediator.Send(timelineListQuery);
            return Ok(dtos);
        }

        [HttpGet("{timelineId}", Name = "GetTimelineById")]
        public async Task<ActionResult<TimelineDetailDto>> GetTimeLineById(Guid timelineId)
        {
            var getTimelineDetailQuery = new GetTimelineDetailQuery() { TimelineId = timelineId };
            return Ok(await _mediator.Send(getTimelineDetailQuery));
        }

        [HttpPost(Name = "AddTimeline")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateTimelineCommand createTimelineCommand)
        {
            var id = await _mediator.Send(createTimelineCommand);
            return Ok(id);
        }

        [HttpPut(Name = "UpdateTimeline")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateTimelineCommand updateTimelineCommand)
        {
            await _mediator.Send(updateTimelineCommand);
            return NoContent();
        }

        [HttpDelete("{timelineId}", Name = "DeleteTimeline")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid timelineId)
        {
            var deleteTimelineCommand = new DeleteTimelineCommand() { TimelineId = timelineId };
            await _mediator.Send(deleteTimelineCommand);
            return NoContent();
        }
    }
}