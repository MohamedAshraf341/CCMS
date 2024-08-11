using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Const;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, IUnitOfWork uow)
        {
            try
            {
                Log.Information("Starting user seeding...");

                var systemAdmin = new ApplicationUser
                {
                    Id = Users.SystemAdmin.Id,
                    Name = Users.SystemAdmin.Name,
                    SystemType = Common.Const.SystemType.System,
                    UserName = Users.SystemAdmin.Email,
                    Email = Users.SystemAdmin.Email,
                    EmailConfirmed = Users.SystemAdmin.ConfirmEmail
                };
                var checksystemAdmin = await userManager.FindByEmailAsync(systemAdmin.Email);
                if (checksystemAdmin == null)
                {
                    var rewadus = await userManager.CreateAsync(systemAdmin, Users.SystemAdmin.Password);
                    await userManager.AddToRoleAsync(systemAdmin, Roles.Admin.Name);
                    Log.Information("System Admin user created.");
                }
                else
                {
                    Log.Information("System Admin user already exists.");
                }

                var systemUser = new ApplicationUser
                {
                    Id = Users.SystemUser.Id,
                    Name = Users.SystemUser.Name,
                    SystemType = Common.Const.SystemType.System,
                    UserName = Users.SystemUser.Email,
                    Email = Users.SystemUser.Email,
                    EmailConfirmed = Users.SystemUser.ConfirmEmail
                };
                var checksystemUser = await userManager.FindByEmailAsync(systemUser.Email);
                if (checksystemUser == null)
                {
                    await userManager.CreateAsync(systemUser, Users.SystemUser.Password);
                    await userManager.AddToRoleAsync(systemUser, Roles.User.Name);
                    Log.Information("System User user created.");
                }
                else
                {
                    Log.Information("System User user already exists.");
                }

                var restaurantAdmin = new ApplicationUser
                {
                    Id = Users.RestaurantAdmin.Id,
                    Name = Users.RestaurantAdmin.Name,
                    SystemType = Common.Const.SystemType.Restaurant,
                    UserName = Users.RestaurantAdmin.Email,
                    Email = Users.RestaurantAdmin.Email,
                    EmailConfirmed = Users.RestaurantAdmin.ConfirmEmail
                };
                var checkRestaurantAdmin = await userManager.FindByEmailAsync(restaurantAdmin.Email);
                if (checkRestaurantAdmin == null)
                {
                    await userManager.CreateAsync(restaurantAdmin, Users.RestaurantAdmin.Password);
                    await userManager.AddToRoleAsync(restaurantAdmin, Roles.Admin.Name);
                    Log.Information("Restaurant Admin user created.");
                }
                else
                {
                    Log.Information("Restaurant Admin user already exists.");
                }

                var restaurantUser = new ApplicationUser
                {
                    Id = Users.RestaurantUser.Id,
                    Name = Users.RestaurantUser.Name,
                    SystemType = Common.Const.SystemType.Restaurant,
                    UserName = Users.RestaurantUser.Email,
                    Email = Users.RestaurantUser.Email,
                    EmailConfirmed = Users.RestaurantUser.ConfirmEmail
                };
                var checkRestaurantUser = await userManager.FindByEmailAsync(restaurantUser.Email);
                if (checkRestaurantUser == null)
                {
                    await userManager.CreateAsync(restaurantUser, Users.RestaurantUser.Password);
                    await userManager.AddToRoleAsync(restaurantUser, Roles.User.Name);
                    Log.Information("Restaurant User user created.");
                }
                else
                {
                    Log.Information("Restaurant User user already exists.");
                }
                var checkRestaurant=await uow.Restaurant.GetAllAsync();
                if (checkRestaurant == null)
                {
                    var branchDefault = new Branch
                    {
                        Id = Guid.Parse("451d02be-a45b-40d0-9640-7de6c8838fd8"),
                        Government = "Banisuef",
                        City = "Banisuef",
                        Area = "Kornesh",
                        Restaurant = new Restaurant { Name = "Pubge" },
                    };
                    branchDefault = await uow.Branche.AddAsync(branchDefault);
                    Log.Information("Default branch created.");

                    var branchUser1 = new BranchUser { BranchId = branchDefault.Id, UserId = restaurantAdmin.Id };
                    await uow.BranchUser.AddAsync(branchUser1);
                    Log.Information("Branch user 1 created.");

                    var branchUser2 = new BranchUser { BranchId = branchDefault.Id, UserId = restaurantUser.Id };
                    await uow.BranchUser.AddAsync(branchUser2);
                    Log.Information("Branch user 2 created.");
                }


                var res = await uow.CompleteAsync();
                Log.Information("Transaction completed successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                //await transaction.RollbackAsync();
                //Log.Information("Transaction rolled back.");
            }

        }
    }
}
