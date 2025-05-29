namespace Gigbuds_BE.Application.DTOs
{
    public class PagedResultDto<T>(int count, IReadOnlyList<T> data) where T : class
    {
        public int Count { get; private set; } = count;
        public IReadOnlyList<T> Data { get; private set; } = data;
    }
}
