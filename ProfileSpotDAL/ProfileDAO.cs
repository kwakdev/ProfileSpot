using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProfileSpotDAL
{
    public class ProfileDAO
    {
        /// <summary>
        /// Retrieves a user profile by last name.
        /// </summary>
        /// <param name="name">The last name of the user.</param>
        /// <returns>A UserProfile object if found; otherwise, null.</returns>
        public async Task<UserProfile> GetByLastname(string name)
        {
            UserProfile? selectedUser;
            try
            {
                ProfileSpotContext _db = new();
                selectedUser = await _db.UserProfiles.FirstOrDefaultAsync(usr => usr.LastName == name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedUser!;
        }

        /// <summary>
        /// Retrieves a user profile by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A UserProfile object if found; otherwise, null.</returns>
        public async Task<UserProfile> GetById(int id)
        {
            UserProfile? selectedUser;
            try
            {
                ProfileSpotContext _db = new();
                selectedUser = await _db.UserProfiles.FindAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedUser!;
        }

        /// <summary>
        /// Retrieves all user profiles.
        /// </summary>
        /// <returns>A list of all UserProfile objects.</returns>
        public async Task<List<UserProfile>> GetAll()
        {
            List<UserProfile> allUsers;
            try
            {
                ProfileSpotContext _db = new();
                allUsers = await _db.UserProfiles.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allUsers;
        }

        /// <summary>
        /// Adds a new user profile.
        /// </summary>
        /// <param name="newUser">The UserProfile object to add.</param>
        /// <returns>The ID of the newly added user profile.</returns>
        public async Task<int> Add(UserProfile newUser)
        {
            try
            {
                ProfileSpotContext _db = new();
                await _db.UserProfiles.AddAsync(newUser);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return newUser.UserId;
        }

        /// <summary>
        /// Updates an existing user profile.
        /// </summary>
        /// <param name="updatedUser">The updated UserProfile object.</param>
        /// <returns>An UpdateStatus indicating the result of the update operation.</returns>
        public async Task<UpdateStatus> Update(UserProfile updatedUser)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                ProfileSpotContext _db = new();
                UserProfile? currentUser =
                await _db.UserProfiles.FirstOrDefaultAsync(usr => usr.UserId == updatedUser.UserId);
                _db.Entry(currentUser!).CurrentValues.SetValues(updatedUser);
                if (await _db.SaveChangesAsync() == 1)
                {
                    status = UpdateStatus.Ok;
                } // should return 1
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
        /// Deletes a user profile by ID.
        /// </summary>
        /// <param name="id">The ID of the user profile to delete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> Delete(int id)
        {
            try
            {
                ProfileSpotContext _dbContext = new();

                // Retrieve the entity by ID
                var entity = await _dbContext.UserProfiles.FindAsync(id);

                if (entity != null)
                {
                    _dbContext.UserProfiles.Remove(entity);
                    return await _dbContext.SaveChangesAsync();
                }
                else
                {
                    Debug.WriteLine($"User with ID {id} not found.");
                    // Handle the case where entity is null (optional)
                    throw new ArgumentNullException(nameof(entity), $"User with ID {id} not found.");
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
