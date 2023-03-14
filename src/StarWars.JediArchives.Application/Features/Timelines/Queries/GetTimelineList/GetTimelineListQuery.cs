namespace StarWars.JediArchives.Application.Features.Timelines.Queries.GetTimelineList
{
    public class GetTimelineListQuery : IParametrizedDto, IRequest<PagedList<TimelineListDto>>
    {
        private int _pageSize;
        private int _pageNumber;

        public readonly int MaxPageSize;
        public readonly int DefaultPageSize;
        public readonly int DefaultPageNumber;

        public bool IsPagingCustomized { get; private set; }
        public int PageNumber 
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = value;
                IsPagingCustomized = true;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
                IsPagingCustomized = true;
            }
        }

        public string QueryString { get; set; }

        public GetTimelineListQuery()
        {
            DefaultPageSize = 10;
            _pageSize = DefaultPageSize;
            MaxPageSize = 50;
            DefaultPageNumber = 1;
            PageNumber = DefaultPageNumber;
            IsPagingCustomized = false;
        }
    }
}