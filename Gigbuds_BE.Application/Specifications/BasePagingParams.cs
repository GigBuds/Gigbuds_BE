namespace Gigbuds_BE.Application.Specifications
{
    internal abstract class BasePagingParams
    {
        // Page Index
        // - Start a 1
        public int PageIndex { get; set; } = 1;

        // Page Size
        private const int _maxPageSize = 20;

        private int _pageSize = 5;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > _maxPageSize ? _maxPageSize : value; }
        }
    }
}
