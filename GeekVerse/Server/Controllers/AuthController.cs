using GeekVerse.Server.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeekVerse.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
           _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var response = await _authService.Register(
                new User 
            { 
                Email = request.Email
            
            }, request.Password);

            if(!response.Success) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            var response = await _authService.Login(
                request.Email, request.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }

            return(response);
        }

        //Pq somente essa api precisa da declaracao [FromBodyRequest] ??
        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string changePassword)
        {
            //tentar usar context.User como feito no client para acessar user. Tb pode usar HttpContext.User
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _authService.ChangePassword(int.Parse(userId), changePassword);

            if(!response.Success)
            {
                return BadRequest(response);
            }


            return Ok(response);
        }

    }
}
