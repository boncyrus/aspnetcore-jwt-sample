using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreJwt.Models
{
    public class GetUsersResponse
    {
        public IEnumerable<UserEntity> Users { get; set; } = Enumerable.Empty<UserEntity>();
    }
}