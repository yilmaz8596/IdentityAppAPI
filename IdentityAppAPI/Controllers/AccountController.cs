using API.Utility;
using IdentityAppAPI.DTOs.Account;
using IdentityAppAPI.Extensions;
using IdentityAppAPI.Models;
using IdentityAppAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            this.configuration = configuration;
        }

        [HttpGet("auth-status")] 

        public IActionResult IsLoggedIn() { 
            return Ok(new {IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
        }

        [HttpPost("register")] 

        public async Task<IActionResult> Register(RegisterDTO model) { 
            if(await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest("Email address is already in use");
            }

            if(await CheckUserNameExistsAsync(model.UserName))
            {
                return BadRequest("Username is already in use");
            }

            var userToAdd = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Registration successful");
        }

        [HttpPost("login")] 
        public async Task<ActionResult<AppUserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.Users.Where(u => u.UserName == model.UserName).FirstOrDefaultAsync();

            if(user == null)
            {
                user = await _userManager.Users.Where(u => u.Email == model.UserName).FirstOrDefaultAsync();
            }

            if(user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if(!result.Succeeded)
            {
                RemoveJwtCookie();
                return Unauthorized("Invalid username or password");
            }
            return CreateAppUserDTO(user);
        }

        [Authorize]
        [HttpGet("current-user")] 

        public async Task<ActionResult<AppUserDTO>> GetCurrentUser() {
            var user = await _userManager.Users.Where(u => u.Id == User.GetUserId()).FirstOrDefaultAsync();

            if(user == null)
            {
                RemoveJwtCookie(); 
                return Unauthorized();
            }

            return CreateAppUserDTO(user);
        }

        [Authorize]
        [HttpPost("logout")] 

        public IActionResult Logout()
        {
            RemoveJwtCookie();
            return Ok("Logged out successfully");
        }
        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }

        private async Task<bool> CheckUserNameExistsAsync(string userName)
        {
            return await  _userManager.Users.AnyAsync(u => u.UserName == userName);
            
        }

        private AppUserDTO CreateAppUserDTO(AppUser user) {
            string jwt = _tokenService.CreateJWT(user);
            SetJwtCookie(jwt);

            return new AppUserDTO
            {
                UserName = user.UserName,
                JWT = jwt
            };
        }

        private void RemoveJwtCookie() {
            Response.Cookies.Delete(SD.IdentityAppCookie);
        }

        private void SetJwtCookie(string jwt) {
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(int.Parse(configuration["JWT:ExpiryInDays"])),
                SameSite = SameSiteMode.None,
            };
            Response.Cookies.Append(SD.IdentityAppCookie, jwt, cookieOptions);
        }
    }
}
