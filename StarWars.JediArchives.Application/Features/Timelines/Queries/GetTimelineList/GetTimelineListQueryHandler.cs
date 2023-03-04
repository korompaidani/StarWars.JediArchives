using AutoMapper;
using MediatR;
using StarWars.JediArchives.Application.Contracts.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryHandler : IRequestHandler<GetTimelineListQuery, List<TimelineListDto>>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;

        public GetTimelineListQueryHandler(ITimelineRepository timelineRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
            _mapper = mapper;
        }

        async public Task<List<TimelineListDto>> Handle(GetTimelineListQuery request, CancellationToken cancellationToken)
        {
            var timelineList = (await _timelineRepository.ListAllAsync()).OrderBy(tl => tl.StartYear);
            return _mapper.Map<List<TimelineListDto>>(timelineList);
        }
    }
}