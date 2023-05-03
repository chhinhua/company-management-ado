
namespace company_management.DTO
{
    public static class UserSession
    {
        public static User LoggedInUser { get; set; }

        public static bool IsUserLoggedIn()
        {
            return LoggedInUser != null;
        }

        public static void LoginUser(User user)
        {
            LoggedInUser = user;
        }

        public static void LogoutUser()
        {
            LoggedInUser = null;
        }
    }
}
