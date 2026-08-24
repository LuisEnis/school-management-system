namespace SchoolManagement.API.DTOs.Common
{
    public class PaginationRequest
    {
        private const int DefaultPageSize = 15;
        private const int MaxPageSize = 100;

        private int _pageSize = DefaultPageSize;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize
                ? MaxPageSize
                : value < 1
                    ? DefaultPageSize
                    : value;
        }
    }
}
