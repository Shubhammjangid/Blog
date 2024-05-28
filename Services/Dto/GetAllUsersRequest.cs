namespace Services.Dto
{
    public class GetAllUsersRequest
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string? SearchTerm { get; set; }
    }
}