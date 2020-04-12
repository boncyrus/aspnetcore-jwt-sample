using System.Collections.Generic;

namespace AspNetCoreJwt.Models
{
    public class UserEntity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<TokenEntity> RefreshTokens { get; set; } = new List<TokenEntity>();
    }
}