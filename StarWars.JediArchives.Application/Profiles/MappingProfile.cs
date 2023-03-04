using AutoMapper;
using StarWars.JediArchives.Application.Features.Timelines.Commands.CreateTimeline;
using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineDetail;
using StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList;
using StarWars.JediArchives.Domain.Models;

namespace StarWars.JediArchives.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Timeline, TimelineDetailDto>().ReverseMap();
            CreateMap<Timeline, TimelineListDto>().ReverseMap();
            CreateMap<Timeline, CreateTimelineCommand>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
