﻿namespace CCMS.Common.Const
{
    public static class Router
    {
        public const string Root = "api/";
        public static class Account
        {
            public const string Prefix = "Account";
            public const string ConfirmEmail = Prefix+"/ConfirmEmail";
            public const string LogIn = Prefix + "/LogIn";
            public const string SendVerificationCode = Prefix + "/SendVerificationCode";
            public const string RefreshToken = Prefix + "/RefreshToken";
            public const string VerifyCode = Prefix + "/VerifyCode";
            public const string GetUserById = Prefix + "/GetUserById";
            public const string UpdateUser = Prefix + "/UpdateUser";
            public const string LogOut = Prefix + "/LogOut";
            public const string ResetPassword = Prefix + "/ResetPassword";

        }
        public static class Admin
        {
            public const string Prefix = "Admin";
            public const string GetUsers = Prefix + "/GetUsers";
            public const string AddUser = Prefix + "/AddUser";
            public const string DeleteUser = Prefix + "/DeleteUser";
        }
        public static class Restaurant
        {
            public const string Prefix = "Restaurant";
            public const string GetRestaurants = Prefix + "/GetRestaurants";
            public const string AddRestaurant = Prefix + "/AddRestaurant";
            public const string EditRestaurant = Prefix + "/EditRestaurant";
            public const string DeleteRestaurant = Prefix + "/DeleteRestaurant";
        }
        public static class BranchPhone
        {
            public const string Prefix = "BranchPhone";
            public const string GetBranchPhones = Prefix + "/GetBranchPhones";
            public const string AddBranchPhone = Prefix + "/AddBranchPhone";
            public const string EditBranchPhone = Prefix + "/EditBranchPhone";
            public const string DeleteBranchPhone = Prefix + "/DeleteRestaurant";
        }
        public static class Branche
        {
            public const string Prefix = "Branche";
            public const string GetBranches = Prefix + "/GetBranches";
            public const string AddBranche = Prefix + "/AddBranche";
            public const string EditBranche = Prefix + "/EditBranche";
            public const string DeleteBranche = Prefix + "/DeleteBranche";
        }
        public static class MenuItem
        {
            public const string Prefix = "MenuItem";
            public const string GetMenuItems = Prefix + "/GetMenuItems";
            public const string AddMenuItem = Prefix + "/AddMenuItem";
            public const string EditMenuItem = Prefix + "/EditMenuItem";
            public const string DeleteMenuItem = Prefix + "/DeleteMenuItem";

        }
        public static class Order
        {
            public const string Prefix = "Order";
            public const string GetOrders = Prefix + "/GetOrders";
            public const string AddOrder = Prefix + "/AddOrder";
            public const string EditOrder = Prefix + "/EditOrder";
            public const string DeleteOrder = Prefix + "/DeleteOrder";

        }
        public static class Client
        {
            public const string Prefix = "Client";
            public const string GetClients = Prefix + "/GetClients";
            public const string AddClient = Prefix + "/AddClient";
            public const string EditClient = Prefix + "/EditClient";
            public const string DeleteClient = Prefix + "/DeleteClient";

        }
    }
}
