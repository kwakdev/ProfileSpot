using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ProfileSpotViewModels;

namespace ProfileSpotWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserLoginViewModel _userLoginViewModel;

        public LoginController()
        {
            _userLoginViewModel = new UserLoginViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            try
            {
                // Authenticate user
                var userLogin = await _userLoginViewModel.GetByUsernameAndPassword(request.Username, request.Password);
                if (userLogin != null)
                {
                    // Return login ID if authentication is successful
                    return Ok(new { LoginId = userLogin.LoginId });
                }
                else
                {
                    return BadRequest("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
