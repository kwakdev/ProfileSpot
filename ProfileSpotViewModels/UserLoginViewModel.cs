using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using ProfileSpotDAL;

namespace ProfileSpotViewModels
{
    public class UserLoginViewModel
    {
        private readonly UserLoginDAO _dao;
        public int LoginId { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? LastLogin { get; set; }

        // Constructor initializes the DAO
        public UserLoginViewModel()
        {
            _dao = new UserLoginDAO();
        }

        /// <summary>
        /// Retrieves a user login by username and password.
        /// </summary>
        /// <param name="username">The username of the user login.</param>
        /// <param name="password">The password of the user login.</param>
        /// <returns>A UserLogin object if found; otherwise, null.</returns>
        public async Task<UserLogin?> GetByUsernameAndPassword(string username, string password)
        {
            try
            {
                return await _dao.GetByUsernameAndPassword(username, password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user login by username and sets the ViewModel properties.
        /// </summary>
        public async Task GetByUsername()
        {
            try
            {
                UserLogin userLogin = await _dao.GetByUsername(Username!);
                LoginId = userLogin.LoginId;
                UserId = userLogin.UserId;
                Username = userLogin.Username;
                Password = userLogin.Password;
                LastLogin = userLogin.LastLogin;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Username = "not found";
            }
            catch (Exception ex)
            {
                Username = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user login by ID and sets the ViewModel properties.
        /// </summary>
        public async Task GetByID()
        {
            try
            {
                UserLogin userLogin = await _dao.GetById(LoginId);
                LoginId = userLogin.LoginId;
                UserId = userLogin.UserId;
                Username = userLogin.Username;
                Password = userLogin.Password;
                LastLogin = userLogin.LastLogin;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Username = "not found";
            }
            catch (Exception ex)
            {
                Username = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Adds a new user login and sets the LoginId property.
        /// </summary>
        public async Task Add()
        {
            LoginId = -1;
            try
            {
                UserLogin newUserLogin = new()
                {
                    UserId = UserId,
                    Username = Username,
                    Password = Password,
                    LastLogin = LastLogin
                };
                LoginId = await _dao.Add(newUserLogin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing user login.
        /// </summary>
        /// <returns>The status of the update operation.</returns>
        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                UserLogin userLogin = new()
                {
                    LoginId = LoginId,
                    UserId = UserId,
                    Username = Username,
                    Password = Password,
                    LastLogin = LastLogin
                };
                updateStatus = -1; // start out with a failed state
                updateStatus = Convert.ToInt16(await _dao.Update(userLogin)); // overwrite status
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }

        /// <summary>
        /// Retrieves a user login by ID and sets the ViewModel properties.
        /// </summary>
        /// <param name="ID">The ID of the user login.</param>
        public async Task GetByID(int? ID)
        {
            try
            {
                UserLogin userLogin = await _dao.GetById(ID);
                LoginId = userLogin.LoginId;
                UserId = userLogin.UserId;
                Username = userLogin.Username;
                Password = userLogin.Password;
                LastLogin = userLogin.LastLogin;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Username = "not found";
            }
            catch (Exception ex)
            {
                Username = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes a user login by ID.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> Delete()
        {
            try
            {
                // Retrieve the user profile by UserId
                await GetByID(UserId);

                // Check if UserId is valid before attempting delete
                if (UserId > 0)
                {
                    // Call DAO to delete by id
                    return await _dao.Delete(UserId);
                }
                else
                {
                    Debug.WriteLine($"User with ID {UserId} not found or invalid.");
                    // Handle the case where UserId is not valid or not found
                    throw new ArgumentException($"User with ID {UserId} not found or invalid.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
    }
}
