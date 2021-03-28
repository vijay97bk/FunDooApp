using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        readonly UserContext userContext;
        private IConfiguration Configuration { get; }
        public UserRL(UserContext context, IConfiguration configuration)
        {
            userContext = context;
            Configuration = configuration;
        }


        public UserModel Get(int id)
        {

            return userContext.Users
                  .FirstOrDefault(e => e.UserId == id);
        }


        public bool Registration(UserModel user)
        {

            UserModel register = new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password
                
            };

            userContext.Users.Add(register);
            userContext.SaveChanges();

            if (userContext.Users
                  .FirstOrDefault(e => e.UserId == register.UserId) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserModel Login(LoginModel login)
        {
            LoginModel login1 = new LoginModel
            {
                Email = login.Email,
                Password = login.Password
            };


            UserModel searchLogin = userContext.Users
                  .Where(e => e.Email.Equals(login1.Email) && e.Password.Equals(login1.Password)).FirstOrDefault(e => e.Email == login.Email);
            

            if (searchLogin != null)
            {

                searchLogin = new UserModel { UserId = searchLogin.UserId, Email = searchLogin.Email, FirstName=searchLogin.FirstName,LastName=searchLogin.LastName };
                return searchLogin;
                
            }
            else
            {
                return searchLogin;
            }

        }

        public string GenerateToken(UserModel login)
        {
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://mysite.com";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {

                  new Claim("UserId", login.UserId.ToString()),
                  new Claim("Email", login.Email),

              };
            var token = new JwtSecurityToken(Configuration[issuer],
               issuer,
               claims,
               expires: DateTime.Now.AddMinutes(120),
               signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ResetPassword(int id, ResetPasswordModel reset)
        {

            ResetPasswordModel password = new ResetPasswordModel
            {
                NewPassword = reset.NewPassword,
                ConfirmPassword = reset.ConfirmPassword,
            };


            if (password.NewPassword == password.ConfirmPassword)
            {

                var dbUser = userContext.Users.FirstOrDefault(s => s.UserId == id);
                dbUser.Password = reset.NewPassword;
                userContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
