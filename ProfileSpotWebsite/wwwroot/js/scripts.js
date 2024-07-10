let userProfileData = {};

async function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const loginError = document.getElementById("loginError");
    loginError.classList.add("hidden");

    try {
        const response = await fetch('/api/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            const loginId = data.loginId;
            document.getElementById("loginSection").classList.add("hidden");
            document.getElementById("profileInfo").classList.remove("hidden");
            await getUserProfile(loginId);
        } else {
            const errorMessage = await response.text();
            loginError.textContent = errorMessage;
            loginError.classList.remove("hidden");
        }
    } catch (error) {
        loginError.textContent = "Error logging in: " + error.message;
        loginError.classList.remove("hidden");
    }
}

function showCreateAccount() {
    document.getElementById("loginSection").classList.add("hidden");
    document.getElementById("createAccountSection").classList.remove("hidden");
}

async function createAccount() {
    const newUsername = document.getElementById("newUsername").value;
    const newPassword = document.getElementById("newPassword").value;
    const createAccountError = document.getElementById("createAccountError");
    createAccountError.classList.add("hidden");

    sessionStorage.setItem('newUsername', newUsername);
    sessionStorage.setItem('newPassword', newPassword);

    document.getElementById("createAccountSection").classList.add("hidden");
    document.getElementById("addUserSection").classList.remove("hidden");
}

async function getUserProfile(userId) {
    try {
        const response = await fetch(`/api/User/${userId}`);
        if (response.ok) {
            userProfileData = await response.json();
            displayUserProfile(userProfileData);
        } else {
            console.error("Failed to get user profile");
        }
    } catch (error) {
        console.error("Error getting user profile: " + error.message);
    }
}

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

async function updateUserProfile() {
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

async function addNewUser() {
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
