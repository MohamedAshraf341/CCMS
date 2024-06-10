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
    }
}
