using AutoMapper;
using StarWars.DataTank.Application.Features.Timelines.Queries.GetTimelineDetail;
using StarWars.DataTank.Domain.Models;

namespace StarWars.DataTank.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Timeline, TimelineDetailDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
