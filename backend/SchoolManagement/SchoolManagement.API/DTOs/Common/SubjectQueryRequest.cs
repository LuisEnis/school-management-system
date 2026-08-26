namespace SchoolManagement.API.DTOs.Common
{
    public class SubjectQueryRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public bool SortDescending { get; set; }
    }
}
