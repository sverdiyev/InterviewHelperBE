﻿using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IUserService
{
    AuthenticateUserResponse AddUser(UserRequest newUser);
    void EditUser(UserUpdateRequest user);
    AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest user);
    User GetUserByEmail(string email);
    bool UserExists(int userId);
    public User GetUserById(int id);
}