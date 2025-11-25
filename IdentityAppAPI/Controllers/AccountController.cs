using API.Utility;
using IdentityAppAPI.DTOs;
using IdentityAppAPI.DTOs.Account;
using IdentityAppAPI.Extensions;
using IdentityAppAPI.Models;
using IdentityAppAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : APICoreController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpGet("auth-status")] 

        public IActionResult IsLoggedIn() { 
            return Ok(new {IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
        }

        [HttpPost("register")] 

        public async Task<IActionResult> Register(RegisterDTO model) { 
            if(await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest(new APIResponse(400, message:"An account with this email already exists!"));
            }

            if(await CheckNameExistsAsync(model.Name))
            {
                return BadRequest(new APIResponse(400, message:"An account with this name already exists!"));
            }

            var userToAdd = new AppUser
            {
                Name = model.Name,
                UserName = model.Name.ToLower(),
                Email = model.Email,
                EmailConfirmed = true,
                LockoutEnabled = true,
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);

            if(!result.Succeeded)
            {
                return BadRequest(new APIResponse(500,
                    message:"Something went wrong! Please try again."
                    ));
            }

            return Ok(new APIResponse(200, message:"Registration successful! You can login now."));
        }

        [HttpGet("name-taken")] 
        public async Task<IActionResult> IsNameTaken([FromQuery] string name) {
            return(Ok(new {isTaken = await CheckNameExistsAsync(name) }));
        }

        [HttpGet("email-taken")]
        public async Task<IActionResult> IsEmailTaken([FromQuery] string email)
        {
            return (Ok(new { isTaken = await CheckEmailExistsAsync(email) }));
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
                return Unauthorized(new APIResponse(401, message: "Invalid username or password!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

            if(!result.Succeeded)
            {
                RemoveJwtCookie();
                return Unauthorized(new APIResponse(401, title:"Account locked",message: SD.AccountLockedMessage(user.LockoutEnd.Value.DateTime), isHTMLEnabled:true, displayByDefault:true));
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
                return Unauthorized(new APIResponse(401));
            }

            return CreateAppUserDTO(user);
        }

        [Authorize]
        [HttpPost("logout")] 

        public IActionResult Logout()
        {
            RemoveJwtCookie();
            return Ok(new APIResponse(200,message:"Logged out successfully"));
        }
        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email);
        }

        private async Task<bool> CheckNameExistsAsync(string name)
        {
            return await  _userManager.Users.AnyAsync(u => u.UserName == name.ToLower());
            
        }

        private AppUserDTO CreateAppUserDTO(AppUser user) {
            string jwt = _tokenService.CreateJWT(user);
            SetJwtCookie(jwt);

            return new AppUserDTO
            {
                Name = user.Name,
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
                Expires = DateTime.UtcNow.AddDays(int.Parse(Configuration["JWT:ExpiryInDays"])),
                SameSite = SameSiteMode.None,
            };
            Response.Cookies.Append(SD.IdentityAppCookie, jwt, cookieOptions);
        }
    }
}
