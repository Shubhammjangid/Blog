namespace Services.Dto
{
    public class LoginRequest
    {
        public required string EmailId { get; set; }
        public required string Password { get; set; }
    }
}