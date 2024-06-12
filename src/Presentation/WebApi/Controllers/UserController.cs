using Core.Application.Interfaces.Services;
using Core.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromBody]CreateUserRequest req)
        {
            var res  = await _identityService.CreateCustomer(req);
            if(res)
                return Ok();  
            else return BadRequest();
        }


        [HttpPost("create/admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateUserRequest req)
        {
            var res = await _identityService.CreateAdmin(req);
            if (res)
                return Ok();
            else return BadRequest();
        }
        [HttpPost("create/role")]
        public async Task<IActionResult> CreateRole(string req)
        {
            var res = await _identityService.CreateRole(req);
            if (res)
                return Ok();
            else return BadRequest();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest req)
        {
            var res = await _identityService.Login(req);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }

        [HttpGet("refresh-token-login")] // host:port/refresh-token-login?refreshToken=token
        public async Task<IActionResult> RefreshTokenLogin([FromQuery]string refreshToken)
        {
            var res = await _identityService.RefreshTokenLogin(refreshToken);
            if (res != null)
                return Ok(res);
            else return BadRequest();
        }
    }

}
 