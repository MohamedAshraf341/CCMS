using CCMS.Common.Models;
using System;

namespace CCMS.Common.Const;

public class Users
{
    public static readonly User SuperAdmin=new User 
    { 
        Id = "0ab5ce5b-19ac-4dca-8866-6f202e6a61bc",
        Name="Super Admin",
        Email="superadmin@domain.com",
        Password= "SuperAdmin@2024" ,
        ConfirmEmail=true
    };
    public static readonly User SystemAdmin = new User
    {
        ConfirmEmail = true,
        Id = "21747b4e-4683-474a-aa97-a7c7602d021e",
        Name = "System Admin",
        Email = "systemadmin@domain.com",
        Password = "SystemAdmin@2024",
        SystemType=SystemType.System,
    };
    public static readonly User SystemUser = new User 
    {
        ConfirmEmail = true,
        Id ="231d3062-5c8d-4708-872c-0d8cf880fec4",
        Name= "System User",
        Email= "systemuser@domain.com",
        Password = "SystemUser@2024",
        SystemType = SystemType.System,
    };
    public static readonly User RestaurantAdmin = new User
    {
        ConfirmEmail = true,
        Id = "10ac74c8-6333-4096-af94-ec84f4892120",
        Name = "Restaurant Admin",
        Email = "restaurantadmin@domain.com",
        Password = "RestaurantAdmin@2024",
        SystemType = SystemType.Restaurant,
    };
    public static readonly User RestaurantUser = new User
    {
        ConfirmEmail = true,
        Id = "9f57b2b4-533f-4e86-9e4c-02de2da926a3",
        Name = "Restaurant User",
        Email = "restaurantuser@domain.com",
        Password = "RestaurantUser@2024",
        SystemType = SystemType.Restaurant,
    };
}
