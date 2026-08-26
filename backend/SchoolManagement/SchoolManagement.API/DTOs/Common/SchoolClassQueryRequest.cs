namespace SchoolManagement.API.DTOs.Common
{
    public class SchoolClassQueryRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public bool SortDescending { get; set; }
    }
}
