using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
        public class UserContext : DbContext
        {
        //protected readonly IConfiguration Config;

        //public UserContext(IConfiguration config)
        //{
        //    Config = config;
        //}
        public UserContext(DbContextOptions options)
                : base(options)
            {
            }
            public DbSet<UserModel> Users { get; set; }
        }
    }

