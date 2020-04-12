namespace AspNetCoreJwt.Models
{
    public class TokenEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        public string Token { get; set; }
    }
}