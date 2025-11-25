namespace IdentityAppAPI.DTOs
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public bool isHTMLEnabled { get; set; }
        public bool DisplayByDefault { get; set; }
        public bool ShowWithToaster { get; set; }
        public object Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public APIResponse(
            int statusCode,
            string title = null,
            string message = null,
            string details = null,
            bool isHTMLEnabled = false,
            bool displayByDefault = false,
            bool showWithToaster = false,
            object data = null,
            IEnumerable<string> errors = null
            )
        {
            StatusCode = statusCode;
            Title = title ?? GetDefaultTitle(statusCode);
            Message = message ?? GetDefaultMessage(statusCode);
            Details = details;
            this.isHTMLEnabled = isHTMLEnabled;
            DisplayByDefault = displayByDefault;
            ShowWithToaster = showWithToaster;
            Data = data;
            Errors = errors;
        }

        private string GetDefaultTitle(int statusCode)
        {
            return statusCode switch
            {
                200 => "Success",
                201 => "Created",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Info"
            };
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "The request was successful.",
                201 => "The resource was created successfully.",
                400 => "The request could not be understood or was missing required parameters.",
                401 => "Authentication failed or user does not have permissions for the desired action.",
                403 => "Access to the requested resource is forbidden.",
                404 => "The requested resource could not be found.",
                500 => "An error occurred on the server.",
                _ => null
            };
        }
    }
}
