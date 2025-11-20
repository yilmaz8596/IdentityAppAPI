
using Microsoft.AspNetCore.Identity;


namespace IdentityAppAPI.Models
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isActive { get; set; } = true;

        // Navigation property for user roles 
        public ICollection<AppUserRoleBridge> Roles { get; set; } 
    }
}
