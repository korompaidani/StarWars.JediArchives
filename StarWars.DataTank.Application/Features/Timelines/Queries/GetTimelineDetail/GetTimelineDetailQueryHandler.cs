using AutoMapper;
using MediatR;
using StarWars.DataTank.Application.Contracts.Persistence;
using StarWars.DataTank.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Features.Timelines.Queries.GetTimelineDetail
{
    public class GetTimelineDetailQueryHandler : IRequestHandler<GetTimelineDetailQuery, TimelineDetailDto>
    {
        private IAsyncRepository<Timeline> _timelineRepository;
        private IAsyncRepository<Category> _categoryRepository;
        private IMapper _mapper;

        public GetTimelineDetailQueryHandler(IAsyncRepository<Timeline> timelineRepository, IAsyncRepository<Category> categoryRepository, IMapper mapper)
        {
            _timelineRepository = timelineRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<TimelineDetailDto> Handle(GetTimelineDetailQuery request, CancellationToken cancellationToken)
        {
            var timeline = await _timelineRepository.GetByIdAsync(request.TimelineId);
            var category = await _categoryRepository.GetByIdAsync(timeline.CategoryId);

            var timelineDetailViewModel = _mapper.Map<TimelineDetailDto>(timeline);
            timelineDetailViewModel.Category = _mapper.Map<CategoryDto>(category);

            return timelineDetailViewModel;
        }
    }
}