using AspNetCoreJwt.Models;
using System.Threading.Tasks;

namespace AspNetCoreJwt.Services
{
    public interface IUserService
    {
        Task<SignUpResponse> SignUp(SignUpRequest request);

        Task<SignInResponse> SignIn(SignInRequest request);

        Task<GetUsersResponse> GetUsers();

        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}