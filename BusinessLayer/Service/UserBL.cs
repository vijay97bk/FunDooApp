using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL

    {
        IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }


        public UserModel Get(int id)
        {
            try
            {

                return this.userRL.Get(id);
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Registration(UserModel user)
        {
            try
            {

                return this.userRL.Registration(user);                
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public string GenerateToken(UserModel login)
        {
            try
            {

                return this.userRL.GenerateToken(login);               
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        public UserModel Login(LoginModel login)
        {
            try
            {

                return this.userRL.Login(login);                 
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public bool ResetPassword(int id, ResetPasswordModel reset)
        {
            try
            {

                return this.userRL.ResetPassword(id, reset);                
            }

            catch (Exception e)
            {
                throw e;
            }
        }
       

    }
}