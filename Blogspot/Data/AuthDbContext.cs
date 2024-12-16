using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogspot.Data
{
    public class AuthDbContext:IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base (options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Send Roles (Super Admin, Admin, User)
            var superAdminRoleId = "065cf089-5197-4f1c-a910-272b4a984a1f";  //super admin id
            var adminRoleId = "b68602f6-51da-4764-9ef3-f038dc410fa2";       //admin id
            var userRoleId = "f5501f75-ea3a-4071-810b-1927c2c6ed0e";        //user id


            var roles=new List<IdentityRole>
            {
                //Admin
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                //Super Admin
                new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp= superAdminRoleId
                },
                //User
                new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp= userRoleId
                }
            };

            //responsible for inserting the roles into DB
            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdmin
            var superAdminId = "5df0f3bc-09d4-4c02-82b2-4c21e6af48bd";

            var superAdminUser = new IdentityUser
            {
                UserName="superadmin@bloggie.com",
                Email="superadmin@bloggie.com",
                NormalizedEmail="superadmin@bloggie.com".ToUpper(),
                NormalizedUserName= "superadmin@bloggie.com".ToUpper(),
                Id=superAdminId
            };
            superAdminUser.PasswordHash=new PasswordHasher<IdentityUser>().HashPassword(superAdminUser,"Superadmin@123");
            
            //responsible for inserting the roles into DB
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add all roles to SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId=adminRoleId,
                    UserId=superAdminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminId,
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
