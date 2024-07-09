using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using ProfileSpotViewModels;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ProfileSpotViewModels.Tests
{
    public class ViewModelTests
    {
        private readonly ITestOutputHelper output;
        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
            public async Task Employee_ComprehensiveVMTest()
        {
            UserProfileViewModel evm = new()
            {
                FirstName = "Somee",
                LastName = "Empleeeoyee",
                Email = "someewee@abc.com",
                Phone = "(777)7777-7777",
            };
            await evm.Add();
            output.WriteLine("New Employee Added - Id = " + evm.UserId);
            int? id = evm.UserId; // need id for delete later
            await evm.GetByID();
            output.WriteLine("New Employee " + id + " Retrieved");
            evm.Phone = "(555)555-1233";
            if (await evm.Update() == 1)
            {
                output.WriteLine("Employee " + id + " phone# was updated to - " +
               evm.Phone);
            }
            else
            {
                output.WriteLine("Employee " + id + " phone# was not updated!");
            }
            evm.Phone = "Another change that should not work";
            if (await evm.Update() == -2)
            {
                output.WriteLine("Employee " + id + " was not updated due to stale data");
            }
            evm = new UserProfileViewModel 
            {
                UserId = (int)id
            };
            // need to reset because of concurrency error
            await evm.GetByID();
            if (await evm.Delete() == 0)
            {
                output.WriteLine("Employee " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Employee " + id + " was not deleted");
            }
            // should throw expected exception
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async ()
           => await evm.GetByID());
        }
        [Fact]
        public async Task TestDelete()
        {
            // Arrange
            UserProfileViewModel vm = new UserProfileViewModel();
            int userIdToDelete = 9;

            // Act
            await vm.GetById(userIdToDelete); // Fetch the user to ensure it exists
            int deleteResult = await vm.Delete(); // Delete the user

            // Assert
            Assert.Equal(1, deleteResult); // Expecting 1 indicating successful deletion
        }
        [Fact]
        public async Task AddUserProfileAndUserLogin_ReturnsValidIds()
        {
            // Arrange - Create a new user profile
            var userProfileViewModel = new UserProfileViewModel
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                Phone = "9876543210",
                AddressLine = "456 Elm St",
                City = "Springfield",
                Province = "State",
                PostalCode = "54321",
                DateOfBirth = new DateOnly(1985, 10, 15),
                IsAdmin = false,
                CreatedAt = DateTime.Now
            };

            // Act - Add the user profile
            var userId = await userProfileViewModel.Add();

            // Assert - Verify that a valid UserProfile ID is returned
            Assert.True(userId > 0);

            // Now, let's add a user login using the obtained UserProfile ID
            var userLoginViewModel = new UserLoginViewModel
            {
                UserId = userId, // Use the UserProfile ID
                Username = "alice",
                Password = "password123",
                LastLogin = DateTime.Now
            };

            // Act - Add the user login
            await userLoginViewModel.Add();

            // Assert - Verify that a valid UserLogin ID is returned
            Assert.True(userLoginViewModel.LoginId > 0);
        }
    }
}
