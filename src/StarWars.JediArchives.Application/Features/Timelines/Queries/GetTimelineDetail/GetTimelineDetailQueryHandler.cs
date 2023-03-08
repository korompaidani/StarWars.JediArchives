using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StarWars.JediArchives.Application.Contracts.Persistence;
using StarWars.JediArchives.Application.Exceptions;
using StarWars.JediArchives.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail
{
    public class GetTimelineDetailQueryHandler : IRequestHandler<GetTimelineDetailQuery, TimelineDetailDto>
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTimelineDetailQueryHandler> _logger;

        public GetTimelineDetailQueryHandler(ITimelineRepository timelineRepository, ICategoryRepository categoryRepository, IMapper mapper, ILogger<GetTimelineDetailQueryHandler> logger)
        {
            _timelineRepository = timelineRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimelineDetailDto> Handle(GetTimelineDetailQuery request, CancellationToken cancellationToken)
        {
            var timeline = await _timelineRepository.GetByIdAsync(request.TimelineId);

            if(timeline is null)
            {
                _logger.LogWarning($"NotFound Exception was thrown: {nameof(Timeline)} regarding to following request id: {request.TimelineId}");
                throw new NotFoundException(nameof(Timeline), request.TimelineId);
            }

            var category = await _categoryRepository.GetByIdAsync(timeline.CategoryId);

            var timelineDetailViewModel = _mapper.Map<TimelineDetailDto>(timeline);
            timelineDetailViewModel.Category = _mapper.Map<CategoryDto>(category);

            return timelineDetailViewModel;
        }
    }
}