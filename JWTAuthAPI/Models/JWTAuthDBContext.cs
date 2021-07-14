using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthAPI.Models
{
    public class JWTAuthDBContext : DbContext
    {
        public JWTAuthDBContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                UserName = "caner",
                Password = "123"
            }, new User
            {
                UserId = 2,
                UserName = "admin",
                Password = "123"
            });
        }
    }
}
