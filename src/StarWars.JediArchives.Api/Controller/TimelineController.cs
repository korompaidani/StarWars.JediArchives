namespace StarWars.JediArchives.Api.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public TimelineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all", Name = "GetAllTimelines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagedList<TimelineListDto>>> GetAllTimelines([FromQuery] GetTimelineListQuery timelineListQuery)
        {
            var timelines = await _mediator.Send(timelineListQuery);

            // TODO: this is not the final place of setting meta, please don't forget moving it to handler
            {
                var path = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";
                var metadata = new
                {
                    timelines.TotalPagesCount,
                    timelines.PageSize,
                    timelines.CurrentPage,
                    timelines.TotalPages,
                    timelines.HasNext,
                    timelines.HasPrevious,
                    FirstPageLink = timelines.GetFirstPageLink(path),
                    LastPageLink = timelines.GetLastPageLink(path),
                    PreviousPageLink = timelines.GetPreviousPageLink(path),
                    NextPageLink = timelines.GetNextPageLink(path),
                    AllPageLink = timelines.GetAllPageLink(path)
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            }

            return Ok(timelines);
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