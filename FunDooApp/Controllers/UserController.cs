using BusinessLayer.Interface;
using CommonLayer;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FunDooApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserBL userBL;
        private readonly AppSetting _appSettings = new AppSetting();
        private IConfiguration Configuration;

        public UsersController(IUserBL userBL, IConfiguration configuration, IOptions<AppSetting> appSettings)
        {
            this.userBL = userBL;
            Configuration = configuration;

            {
                _appSettings = appSettings.Value;
            }
        }
       
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                UserModel result = userBL.Get(id);
                return this.Ok(new { Success = true, Message = "Get Successful", Data = result });
            }

            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Registration(UserModel user)
        {
            if (ModelState.IsValid)
            {

                bool result = this.userBL.Registration(user);
                if (result != false)
                {
                    return this.Ok(new { Success = true, Message = "Register Contact Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, Message = "Register Contact Unsuccessfully" });
                }
            }

            else
            {
                throw new Exception("Model is not valid");
            }
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var result = this.userBL.Login(user);

                if (user == null)
                    return BadRequest(new { message = "email or password is incorrect" });
                var tokenString = this.userBL.GenerateToken(result);
                if (user != null)
                {
                    return Ok(new
                    {
                        Id = result.UserId,
                        FirstName = result.FirstName,
                        LastName = result.LastName,
                        Token = tokenString
                    });
                }
                else
                {
                    return this.BadRequest(new { Success = false, Message = "Login Failed" });
                }
            }
            else
            {
                throw new Exception("Model is not valid");
            }
        }

        [Authorize]
        [HttpPost("reset")]
        public IActionResult ResetPassword(int id, ResetPasswordModel reset)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var UserId = claims.Where(x => x.Type == "UserId").FirstOrDefault()?.Value;
                    var Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    bool result = this.userBL.ResetPassword(id, reset);
                    if (result)
                    {
                        return Ok(new { success = true, Message = "reset password successfull" });
                    }
                }
                return BadRequest(new { success = false, Message = "password reset failed" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }

       
    }
}


