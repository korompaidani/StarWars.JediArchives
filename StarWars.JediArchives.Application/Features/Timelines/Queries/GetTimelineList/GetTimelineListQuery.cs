using MediatR;
using StarWars.JediArchives.Application.Features.Common.Dto;
using System.Collections.Generic;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQuery : IPageDto, IRequest<List<TimelineListDto>>
    {
        public int? Count { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }
}