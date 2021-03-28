using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserModel Get(int id);

        public bool Registration(UserModel user);

        public UserModel Login(LoginModel user);

        public string GenerateToken(UserModel login);

        public bool ResetPassword(int id, ResetPasswordModel reset);
        
    }
}
