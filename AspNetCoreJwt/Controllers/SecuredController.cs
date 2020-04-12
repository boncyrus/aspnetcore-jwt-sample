using AspNetCoreJwt.Models;
using AspNetCoreJwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreJwt.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SecuredController : Controller
    {
        private readonly IUserService _userService;

        public SecuredController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }
    }
}