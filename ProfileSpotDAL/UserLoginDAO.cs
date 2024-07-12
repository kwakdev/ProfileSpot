using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ProfileSpotDAL
{
    public class UserLoginDAO
    {
        /// <summary>
        /// Retrieves a user login by username.
        /// </summary>
        /// <param name="username">The username of the user login.</param>
        /// <returns>A UserLogin object if found; otherwise, null.</returns>
        public async Task<UserLogin> GetByUsername(string username)
        {
            UserLogin? selectedUserLogin;
            try
            {
                ProfileSpotContext _db = new();
                selectedUserLogin = await _db.UserLogins.FirstOrDefaultAsync(usr => usr.Username == username);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedUserLogin!;
        }

        /// <summary>
        /// Retrieves a user login by ID.
        /// </summary>
        /// <param name="id">The ID of the user login.</param>
        /// <returns>A UserLogin object if found; otherwise, null.</returns>
        public async Task<UserLogin> GetById(int? id)
        {
            UserLogin? selectedUserLogin;
            try
            {
                ProfileSpotContext _db = new();
                selectedUserLogin = await _db.UserLogins.FindAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedUserLogin!;
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
                using (var _db = new ProfileSpotContext())
                {
                    return await _db.UserLogins.FirstOrDefaultAsync(usr => usr.Username == username && usr.Password == password);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all user logins.
        /// </summary>
        /// <returns>A list of all UserLogin objects.</returns>
        public async Task<List<UserLogin>> GetAll()
        {
            List<UserLogin> allUserLogins;
            try
            {
                ProfileSpotContext _db = new();
                allUserLogins = await _db.UserLogins.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allUserLogins;
        }

        /// <summary>
        /// Adds a new user login.
        /// </summary>
        /// <param name="newUserLogin">The UserLogin object to add.</param>
        /// <returns>The ID of the newly added user login.</returns>
        public async Task<int> Add(UserLogin newUserLogin)
        {
            try
            {
                ProfileSpotContext _db = new();
                await _db.UserLogins.AddAsync(newUserLogin);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return newUserLogin.LoginId;
        }

        /// <summary>
        /// Updates an existing user login.
        /// </summary>
        /// <param name="updatedUserLogin">The updated UserLogin object.</param>
        /// <returns>An UpdateStatus indicating the result of the update operation.</returns>
        public async Task<UpdateStatus> Update(UserLogin updatedUserLogin)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                ProfileSpotContext _db = new();
                UserLogin? currentUserLogin = await _db.UserLogins.FirstOrDefaultAsync(usr => usr.LoginId == updatedUserLogin.LoginId);
                _db.Entry(currentUserLogin!).CurrentValues.SetValues(updatedUserLogin);
                if (await _db.SaveChangesAsync() == 1)
                {
                    status = UpdateStatus.Ok;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                status = UpdateStatus.Stale;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return status;
        }

        /// <summary>
        /// Deletes a user login by ID.
        /// </summary>
        /// <param name="id">The ID of the user login to delete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> Delete(int? id)
        {
            int usersDeleted = -1;
            try
            {
                ProfileSpotContext _db = new();
                UserLogin? selectedUserLogin = await _db.UserLogins.FirstOrDefaultAsync(usr => usr.LoginId == id);
                _db.UserLogins.Remove(selectedUserLogin!);
                usersDeleted = await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return usersDeleted;
        }
    }
}
