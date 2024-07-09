using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProfileSpotDAL;
namespace ProfileSpotViewModels
{
    public class UserProfileViewModel
    {
        readonly private ProfileDAO _dao;
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Timer { get; set; }

        public bool? IsAdmin { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime? CreatedAt { get; set; }
        // Additional properties as needed for the UI
        public string? Picture64 { get; set; }
        public int LoginId { get; set; }


     
        public UserProfileViewModel()
        {
            _dao = new ProfileDAO(); 
        }

        public async Task GetByLastname()
        {
            try
            {
                UserProfile usr = await _dao.GetByLastname(LastName!);

                FirstName = usr.FirstName;
                LastName = usr.LastName;
                Email = usr.Email;
                Phone = usr.Phone;
                AddressLine = usr.AddressLine;
                City = usr.City;
                Province = usr.Province;
                PostalCode = usr.PostalCode;
                UserId = usr.UserId;
                DateOfBirth = usr.DateOfBirth;
                CreatedAt = usr.CreatedAt;
                IsAdmin = usr.IsAdmin;
                LoginId = usr.UserId;


                if (usr.Picture != null)
                {
                    Picture64 = Convert.ToBase64String(usr.Picture);
                }
                Timer = Convert.ToBase64String(usr.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                LastName = "not found";
            }
            catch (Exception ex)
            {
                LastName = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task GetByID()
        {
            try
            {
                UserProfile usr = await _dao.GetById(UserId!);

                FirstName = usr.FirstName;
                LastName = usr.LastName;
                Phone = usr.Phone;
                AddressLine = usr.AddressLine;
                City = usr.City;
                Province = usr.Province;
                PostalCode = usr.PostalCode;
                UserId = usr.UserId;
                DateOfBirth = usr.DateOfBirth;
                CreatedAt = usr.CreatedAt;
                IsAdmin = usr.IsAdmin;
                LoginId = usr.UserId;


                if (usr.Picture != null)
                {
                    Picture64 = Convert.ToBase64String(usr.Picture);
                }
                Timer = Convert.ToBase64String(usr.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                LastName = "not found";
            }
            catch (Exception ex)
            {
                LastName = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task<List<UserProfileViewModel>> GetAll()
        {
            List<UserProfileViewModel> allVms = new();
            try
            {
                List<UserProfile> allUsers = await _dao.GetAll();
                
                foreach (UserProfile usr in allUsers)
                {
                    UserProfileViewModel usrVm = new()
                    {
                        FirstName = usr.FirstName,
                    LastName = usr.LastName,
                    Phone = usr.Phone,
                    AddressLine = usr.AddressLine,
                    City = usr.City,
                    Province = usr.Province,
                    PostalCode = usr.PostalCode,
                    UserId = usr.UserId,
                    DateOfBirth = usr.DateOfBirth,
                    CreatedAt = usr.CreatedAt,
                    IsAdmin = usr.IsAdmin,
                    LoginId = usr.UserId,


                 
                    Timer = Convert.ToBase64String(usr.Timer),
                };
                    allVms.Add(usrVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }
        public async Task<int> Add()
        {
            try
            {
                UserProfile stu = new()
                {
                    // Do not set UserId here
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Email = this.Email,
                    Phone = this.Phone,
                    AddressLine = this.AddressLine,
                    City = this.City,
                    Province = this.Province,
                    PostalCode = this.PostalCode,
                    DateOfBirth = this.DateOfBirth,
                    
                    Timer = !string.IsNullOrEmpty(this.Timer) ? Convert.FromBase64String(this.Timer) : null,
                    IsAdmin = this.IsAdmin,
                    Picture = !string.IsNullOrEmpty(this.Picture64) ? Convert.FromBase64String(this.Picture64) : null,
                    CreatedAt = this.CreatedAt,
                };
                int userId = await _dao.Add(stu);
                return userId; // Return the ID of the newly added user
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<int> Delete()
        {
            try
            {
                // Retrieve the user profile by UserId
                await GetById(UserId);

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

        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                UserProfile usr = new()
                {

                    FirstName = FirstName,
                    LastName = LastName,
                    Phone = Phone,
                    AddressLine = AddressLine,
                    City = City,
                    Email = Email,

                    Province = Province,
                    PostalCode = PostalCode,
                    UserId = UserId,
                    DateOfBirth = DateOfBirth,
                    CreatedAt = CreatedAt,
                    IsAdmin = IsAdmin,

                    Picture = Picture64 != null ? Convert.FromBase64String(Picture64!) : null,
                    Timer = Convert.FromBase64String(Timer!)
                };
                updateStatus = -1; // start out with a failed state
                updateStatus = Convert.ToInt16(await _dao.Update(usr)); // overwrite status
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }
        public async Task GetById(int userId)
{
    try
    {
        UserProfile usr = await _dao.GetById(userId);

        FirstName = usr.FirstName;
        LastName = usr.LastName;
        Email = usr.Email;
        Phone = usr.Phone;
        AddressLine = usr.AddressLine;
        City = usr.City;
        Province = usr.Province;
        PostalCode = usr.PostalCode;
        DateOfBirth = usr.DateOfBirth;
        CreatedAt = usr.CreatedAt;
        IsAdmin = usr.IsAdmin;
        LoginId = usr.UserId;

        if (usr.Picture != null)
        {
            Picture64 = Convert.ToBase64String(usr.Picture);
        }
        Timer = Convert.ToBase64String(usr.Timer);
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


