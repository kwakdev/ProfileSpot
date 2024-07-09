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
