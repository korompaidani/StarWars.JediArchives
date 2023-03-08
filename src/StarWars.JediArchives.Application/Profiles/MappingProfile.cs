namespace StarWars.JediArchives.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Timeline, TimelineDetailDto>().ReverseMap();
            CreateMap<Timeline, TimelineListDto>().ReverseMap();
            CreateMap<Timeline, CreateTimelineCommand>().ReverseMap();
            CreateMap<Timeline, UpdateTimelineCommand>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
