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
            CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
        }
    }

    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new PagedList<TDestination>();
            }
            foreach (var item in source)
            {
                var dest = context.Mapper.Map<TSource, TDestination>(item);
                destination.Add(dest);
            }
            destination.TotalCount = source.TotalCount;
            destination.PageSize = source.PageSize;
            destination.CurrentPage = source.CurrentPage;

            return destination;
        }
    }
}
