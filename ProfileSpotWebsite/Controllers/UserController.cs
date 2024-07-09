using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using ProfileSpotViewModels;

namespace ProfileSpotWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserProfileViewModel _userProfileViewModel;
        private readonly UserLoginViewModel _userLoginViewModel;

        public UserController()
        {
            _userProfileViewModel = new UserProfileViewModel();
            _userLoginViewModel = new UserLoginViewModel();
        }

        // PUT /api/User/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileViewModel updatedProfile)
        {
            try
            {
                UserProfileViewModel viewModel = new UserProfileViewModel();
                viewModel.UserId = userId;

                // Update properties from the provided data
                viewModel.FirstName = updatedProfile.FirstName;
                viewModel.LastName = updatedProfile.LastName;
                viewModel.Phone = updatedProfile.Phone;
                viewModel.AddressLine = updatedProfile.AddressLine;
                viewModel.City = updatedProfile.City;
                viewModel.Email = updatedProfile.Email;
                viewModel.Province = updatedProfile.Province;
                viewModel.PostalCode = updatedProfile.PostalCode;
                viewModel.DateOfBirth = updatedProfile.DateOfBirth;
                viewModel.IsAdmin = updatedProfile.IsAdmin;

                int rowsUpdated = await viewModel.Update();

                if (rowsUpdated > 0)
                {
                    return Ok("User profile updated successfully!");
                }
                else
                {
                    return NotFound("User not found or could not be updated.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // GET /api/User/lastname/{lastname}
        [HttpGet("lastname/{lastname}")]
        public async Task<IActionResult> GetByLastname(string lastname)
        {
            try
            {
                UserProfileViewModel viewmodel = new() { LastName = lastname };
                await viewmodel.GetByLastname();
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // GET /api/User/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                UserProfileViewModel viewmodel = new();
                viewmodel.UserId = userId;
                await viewmodel.GetById(userId);
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        // POST /api/User
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserWithLoginRequest request)
        {
            try
            {
                // Create a new instance of UserProfileViewModel
                var userProfileViewModel = new UserProfileViewModel();

                // Set properties for user profile
                userProfileViewModel.FirstName = request.FirstName;
                userProfileViewModel.LastName = request.LastName;
                userProfileViewModel.Email = request.Email;
                userProfileViewModel.Phone = request.Phone;
                userProfileViewModel.AddressLine = request.AddressLine;
                userProfileViewModel.City = request.City;
                userProfileViewModel.Province = request.Province;
                userProfileViewModel.PostalCode = request.PostalCode;
                userProfileViewModel.IsAdmin = request.IsAdmin;
                userProfileViewModel.CreatedAt = DateTime.Now;

                // Add user profile
                int userId = await userProfileViewModel.Add();

                // Create a new instance of UserLoginViewModel
                var userLoginViewModel = new UserLoginViewModel();

                // Set properties for user login
                userLoginViewModel.UserId = userId;
                userLoginViewModel.Username = request.Username;
                userLoginViewModel.Password = request.Password;

                // Add user login
                await userLoginViewModel.Add();

                return Ok("User added successfully with login.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Debug.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
                return StatusCode(500, $"An error occurred: {ex.Message}. See the inner exception for details: {ex.InnerException?.Message}");
            }
        }

        // DELETE /api/User/{userId}
        // DELETE /api/User/{userId}
        // DELETE /api/User/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserProfile(int userId)
        {
            try
            {
                // Delete user login first
                var userLoginViewModel = new UserLoginViewModel();
                userLoginViewModel.UserId = userId;
                await userLoginViewModel.Delete(); // Assuming you have a Delete method in UserLoginViewModel

                // Then delete user profile
                var userProfileViewModel = new UserProfileViewModel();
                userProfileViewModel.UserId = userId;
                int rowsDeleted = await userProfileViewModel.Delete();

                if (rowsDeleted > 0)
                {
                    return Ok("User profile and login information deleted successfully!");
                }
                else
                {
                    return NotFound("User profile not found or could not be deleted.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public class UserWithLoginRequest
        {
            // User profile properties
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string? Phone { get; set; }
            public string? AddressLine { get; set; }
            public string? City { get; set; }
            public string? Province { get; set; }
            public string? PostalCode { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public bool? IsAdmin { get; set; }

            // User login properties
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
