let userProfileData = {}; // Object to store user profile data

// Function to handle login
async function login() {
    const username = document.getElementById("username").value; // Get username input
    const password = document.getElementById("password").value; // Get password input
    const loginError = document.getElementById("loginError"); // Get login error element
    loginError.classList.add("hidden"); // Hide login error initially

    try {
        // Send login request to server
        const response = await fetch('/api/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            // If login is successful, get the user profile
            const data = await response.json();
            const loginId = data.loginId;
            document.getElementById("loginSection").classList.add("hidden");
            document.getElementById("profileInfo").classList.remove("hidden");
            await getUserProfile(loginId);
        } else {
            // If login fails, display the error message
            const errorMessage = await response.text();
            loginError.textContent = errorMessage;
            loginError.classList.remove("hidden");
        }
    } catch (error) {
        // If there is an error in the login process, display the error message
        loginError.textContent = "Error logging in: " + error.message;
        loginError.classList.remove("hidden");
    }
}

// Function to show the create account section
function showCreateAccount() {
    document.getElementById("loginSection").classList.add("hidden");
    document.getElementById("createAccountSection").classList.remove("hidden");
}

// Function to handle the creation of a new account
async function createAccount() {
    const newUsername = document.getElementById("newUsername").value; // Get new username input
    const newPassword = document.getElementById("newPassword").value; // Get new password input
    const createAccountError = document.getElementById("createAccountError"); // Get create account error element
    createAccountError.classList.add("hidden"); // Hide create account error initially

    // Store the new username and password in session storage
    sessionStorage.setItem('newUsername', newUsername);
    sessionStorage.setItem('newPassword', newPassword);

    // Show the add user section and hide the create account section
    document.getElementById("createAccountSection").classList.add("hidden");
    document.getElementById("addUserSection").classList.remove("hidden");
}

// Function to get the user profile
async function getUserProfile(userId) {
    try {
        const response = await fetch(`/api/User/${userId}`);
        if (response.ok) {
            // If request is successful, display the user profile
            userProfileData = await response.json();
            displayUserProfile(userProfileData);
        } else {
            console.error("Failed to get user profile");
        }
    } catch (error) {
        console.error("Error getting user profile: " + error.message);
    }
}

// Function to display the user profile in the form
function displayUserProfile(userProfile) {
    document.getElementById("firstName").value = userProfile.firstName || "";
    document.getElementById("lastName").value = userProfile.lastName || "";
    document.getElementById("phone").value = userProfile.phone || "";
    document.getElementById("addressLine").value = userProfile.addressLine || "";
    document.getElementById("city").value = userProfile.city || "";
    document.getElementById("email").value = userProfile.email || "";
    document.getElementById("province").value = userProfile.province || "";
    document.getElementById("postalCode").value = userProfile.postalCode || "";
    document.getElementById("dateOfBirth").value = userProfile.dateOfBirth || "";
    document.getElementById("createdAt").value = userProfile.createdAt || "";
    document.getElementById("isAdmin").checked = userProfile.isAdmin || false;
}

// Function to update the user profile
async function updateUserProfile() {
    // Create updated profile object from form inputs
    const updatedProfile = {
        ...userProfileData,
        firstName: document.getElementById("firstName").value,
        lastName: document.getElementById("lastName").value,
        phone: document.getElementById("phone").value,
        addressLine: document.getElementById("addressLine").value,
        city: document.getElementById("city").value,
        email: document.getElementById("email").value,
        province: document.getElementById("province").value,
        postalCode: document.getElementById("postalCode").value,
        dateOfBirth: document.getElementById("dateOfBirth").value,
        isAdmin: document.getElementById("isAdmin").checked,
    };

    try {
        const response = await fetch(`/api/User/${userProfileData.userId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedProfile),
        });

        if (response.ok) {
            alert("Profile updated successfully");
        } else {
            alert("Failed to update profile");
        }
    } catch (error) {
        alert("Error updating profile: " + error.message);
    }
}

// Function to delete the user profile
async function deleteUserProfile() {
    try {
        const response = await fetch(`/api/User/${userProfileData.userId}`, {
            method: 'DELETE',
        });

        if (response.ok) {
            alert("Profile deleted successfully");
            document.getElementById("profileInfo").classList.add("hidden");
            document.getElementById("loginSection").classList.remove("hidden");
        } else {
            alert("Failed to delete profile");
        }
    } catch (error) {
        alert("Error deleting profile: " + error.message);
    }
}

// Function to add a new user
async function addNewUser() {
    // Create new user profile object from form inputs
    const newUserProfile = {
        username: sessionStorage.getItem('newUsername'),
        password: sessionStorage.getItem('newPassword'),
        firstName: document.getElementById("newFirstName").value,
        lastName: document.getElementById("newLastName").value,
        phone: document.getElementById("newPhone").value,
        addressLine: document.getElementById("newAddressLine").value,
        city: document.getElementById("newCity").value,
        email: document.getElementById("newEmail").value,
        province: document.getElementById("newProvince").value,
        postalCode: document.getElementById("newPostalCode").value,
        isAdmin: document.getElementById("newIsAdmin").checked,
    };

    try {
        const response = await fetch(`/api/User`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newUserProfile),
        });

        if (response.ok) {
            alert("New user added successfully");
            document.getElementById("addUserSection").classList.add("hidden");
            document.getElementById("loginSection").classList.remove("hidden");
        } else {
            const errorMessage = await response.text();
            document.getElementById("addUserError").textContent = errorMessage;
            document.getElementById("addUserError").classList.remove("hidden");
        }
    } catch (error) {
        document.getElementById("addUserError").textContent = "Error adding user: " + error.message;
        document.getElementById("addUserError").classList.remove("hidden");
    }
}
