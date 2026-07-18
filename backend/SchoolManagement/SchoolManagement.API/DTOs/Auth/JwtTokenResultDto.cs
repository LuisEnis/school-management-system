namespace SchoolManagement.API.DTOs.Auth
{
    public class JwtTokenResultDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }
    }
}
