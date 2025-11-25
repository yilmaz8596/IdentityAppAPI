using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAppAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        [HttpGet("get-players")] 
         
        public IActionResult GetPlayers()
        {
            var players = new[]
            {
                new { Id = 1, Name = "PlayerOne", Score = 1500 },
                new { Id = 2, Name = "PlayerTwo", Score = 2000 },
                new { Id = 3, Name = "PlayerThree", Score = 2500 }
            };
            return Ok(players);
        }
    }
}
