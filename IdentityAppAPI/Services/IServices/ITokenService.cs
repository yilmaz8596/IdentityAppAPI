using IdentityAppAPI.Models;

namespace IdentityAppAPI.Services.IServices
{
    public interface ITokenService
    {
        string CreateJWT(AppUser user);
    }
}
