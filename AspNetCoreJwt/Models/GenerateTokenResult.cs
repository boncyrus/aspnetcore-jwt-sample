using System;

namespace AspNetCoreJwt.Models
{
    public class GenerateTokenResult
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}