using AutoMapper;
using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQueryHandler : IRequestHandler<GetTimelineListQuery, List<TimelineListDto>>
    {
        private readonly IAsyncRepository<Timeline> _timelineRepository;
        private readonly IMapper _mapper;

        public GetTimelineListQueryHandler(IAsyncRepository<Timeline> timelineRepository, IMapper mapper)
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