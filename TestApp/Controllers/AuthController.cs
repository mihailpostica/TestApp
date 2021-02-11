using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.DTO.Requests;
using Test.DTO.Responses;

namespace Test
{

    public partial class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody]RegisterRequest registerRequest)
        {
            var registerResult = await _authService.RegisterUserAsync(registerRequest);
            return SendAuthResult(registerResult);
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody]LoginRequest loginRequest) 
        {
            var authResult=await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);
            return SendAuthResult(authResult);
        }

        [HttpPost("googleLogin")]
        public async Task<IActionResult> googleLogin([FromBody]TokenDTO id_token)
        {
            var authResult = await _authService.LoginWithGoogleAsync(id_token.token);
            return SendAuthResult(authResult);
        }

        private IActionResult SendAuthResult(AuthResult authResult)
        {
            if (authResult.Success)
            {
                return Ok(new AuthSuccessResponse { Token = authResult.Token });
            }
            else
            {
                return BadRequest(new AuthFailedResponse { Errors = authResult.Errors });
            }
        }
    }
}
