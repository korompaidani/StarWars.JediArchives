namespace StarWars.JediArchives.Application.Features.Common.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages 
        {
            get
            {
                return (int)Math.Ceiling(TotalPagesCount / (double)PageSize);
            }
        }

        public int PageSize { get; set; }
        public int TotalPagesCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// TODO: Temporary solution because my custom converter needs this to ba able to map
        /// </summary>
        public PagedList() 
        {
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalPagesCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;

            AddRange(items);
        }

        public string GetNextPageLink(string routeTemplate)
        {
            return HasNext ? $"{routeTemplate}?pageNumber={CurrentPage + 1}&pageSize={PageSize}" : null;
        }

        public string GetPreviousPageLink(string routeTemplate)
        {
            return HasPrevious ? $"{routeTemplate}?pageNumber={CurrentPage - 1}&pageSize={PageSize}" : null;
        }

        public string GetFirstPageLink(string routeTemplate)
        {
            return $"{routeTemplate}?pageNumber={1}&pageSize={PageSize}";
        }

        public string GetLastPageLink(string routeTemplate)
        {
            return $"{routeTemplate}?pageNumber={TotalPages}&pageSize={PageSize}";
        }

        public string GetAllPageLink(string routeTemplate)
        {
            return $"{routeTemplate}";
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize, bool isPagingIgnored = false)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * (isPagingIgnored ? 0 : pageSize)).Take(isPagingIgnored ? count : pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
