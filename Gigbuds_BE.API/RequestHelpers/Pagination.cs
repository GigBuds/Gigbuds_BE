namespace Gigbuds_BE.API.RequestHelpers
{
    public class Pagination<T>(IReadOnlyList<T> items,  int count, int pageIndex, int pageSize)
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public IReadOnlyList<T> Items { get; set; } = items;
    }
}
