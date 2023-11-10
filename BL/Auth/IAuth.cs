﻿using hh_clone.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace hh_clone.BL.Auth
{
    public interface IAuth
    {
        Task<int> CreateUser(UserModel user);

        Task<int?> Authenticate(string email, string password, bool rememberMe);

        Task ValidateEmail(string email);

        Task Register(UserModel user);

	}
}
