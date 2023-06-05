namespace ZingTickets.UserManagement.Controllers
{
    internal class TokenResponseModel
    {
        public DateTimeOffset expires { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}