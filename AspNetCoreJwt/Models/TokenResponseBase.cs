using System;

namespace AspNetCoreJwt.Models
{
    public class TokenResponseBase
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}