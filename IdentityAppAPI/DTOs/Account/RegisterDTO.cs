using API.Utility;
using System.ComponentModel.DataAnnotations;


namespace IdentityAppAPI.DTOs.Account
{
    public class RegisterDTO
    {
        private string _userName;

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage ="Username must be at least {2} and maximum {1} characters.")]
        [RegularExpression(SD.UserNameRegex, ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        
        public string UserName
        {
            get => _userName;
            set => _userName = value?.ToLower();
        }

        [Required]
        [RegularExpression(SD.EmailRegex, ErrorMessage = "Invalid email address.")]

        private string _email;

        public string Email
        {
            get => _email;
            set => _email = value?.ToLower();
        }

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be at least {2} and maximum {1} characters.")]
        public string Password { get; set; }
    }
}
