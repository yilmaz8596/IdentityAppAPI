namespace API.Utility
{
    public static class SD
    {
        public static readonly string IdentityAppCookie = "identityappcookie";
        public const string UserId = "uid"; 
        public const string UserName = "username";
        public const string Email = "email";
        public const string UserNameRegex = @"^[a-zA-Z0-9_]{3,20}$";
        public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const int RequiredLength = 6;
        public const int MaxFailedAccessAttemps = 3;
        public const int DefaultLockoutTimeSpan = 1; // in days
    }
}
