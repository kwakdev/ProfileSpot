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
        public async Task<UserProfile> GetByLastname(string name)
        {
            UserProfile? selectedUser;
            try
            {
                ProfileSpotContext _db = new();
                selectedUser = await _db.UserProfiles.FirstOrDefaultAsync(usr => usr.LastName ==
               name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedUser!;
        }

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
                }// should return 1
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
      
        public async Task<int> Delete(int id)
        {
            try
            {
                ProfileSpotContext _dbContext = new();

                // Retrieve the entity by id
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

