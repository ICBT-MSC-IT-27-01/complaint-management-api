namespace Cd.Cms.Shared
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
        public T[] Items { get; set; } = Array.Empty<T>();
    }
}
