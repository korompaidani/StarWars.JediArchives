using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList;
using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail;
using StarWars.JediArchives.Application.Features.Timelines.Commands.CreateTimeline;
using StarWars.JediArchives.Application.Features.Timelines.Commands.UpdateTimeline;
using StarWars.JediArchives.Application.Features.Timelines.Commands.DeleteTimeline;

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
        public async Task<ActionResult<List<TimelineListDto>>> GetAllTimelines()
        {
            var dtos = await _mediator.Send(new GetTimelineListQuery());
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