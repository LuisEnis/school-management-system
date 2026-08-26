namespace SchoolManagement.API.DTOs.Common
{
    public class UserQueryRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; }
    }
}
