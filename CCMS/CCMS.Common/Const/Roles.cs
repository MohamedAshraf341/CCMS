using CCMS.Common.Models;
using System;

namespace CCMS.Common.Const
{
    public static class Roles
    {
        public static readonly Role SuperAdmin = new Role { Id = "2d900cf8-53ec-4f5d-84f1-394f29ed616c",Name= "SuperAdmin" };
        public static readonly Role Admin = new Role { Id = "db11b0c6-8e18-4535-8e17-c77520374812", Name = "Admin" };
        public static readonly Role User = new Role { Id = "08250abe-3fc4-4be5-9cff-fe850cb92204", Name = "User" };

    }
}
