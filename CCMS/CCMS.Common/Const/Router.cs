namespace CCMS.Common.Const
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
            public const string VerifyCode = Prefix + "/VerifyCode";
            public const string GetUserById = Prefix + "/GetUserById";
            public const string UpdateUser = Prefix + "/UpdateUser";
            public const string LogOut = Prefix + "/LogOut";

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
    }
}
