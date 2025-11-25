using System;

namespace API.Utility
{
    public static class SD
    {
        public static readonly string IdentityAppCookie = "identityappcookie";
        public const string UserId = "uid"; 
        public const string UserName = "username";
        public const string Name = "name";
        public const string Email = "email";
        public const string UserNameRegex = @"^[a-zA-Z][a-zA-Z0-9_]{3,20}$";
        public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const int RequiredLength = 6;
        public const int MaxFailedAccessAttemps = 3;
        public const int DefaultLockoutTimeSpan = 1; // in days
        public const string DefaultPassword = "123456";
        public static string AccountLockedMessage(DateTime endDate)
        {
            DateTime startDate = DateTime.UtcNow; 
            TimeSpan difference = endDate - startDate;
            int days = difference.Days;
            int hours = difference.Hours;
            int minutes = difference.Minutes + 1; 

            return string.Format($"Your account is locked. Please try again after {0} day(s), {1} hour(s) and  {2} minute(s).");
        }
    }
}
