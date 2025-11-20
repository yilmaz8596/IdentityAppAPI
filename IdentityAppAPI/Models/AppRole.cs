using Microsoft.AspNetCore.Identity;

namespace IdentityAppAPI.Models
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRoleBridge> Users { get; set; }
    }
}
