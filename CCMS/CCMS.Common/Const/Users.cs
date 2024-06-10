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
}
