using IdentityAppAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APICoreController : ControllerBase
    {
        private IConfiguration _config;

        private Context _context;
        protected Context context => _context ??= HttpContext.RequestServices.GetService<Context>();
        protected IConfiguration Configuration => _config ??= HttpContext.RequestServices.GetService<IConfiguration>();

    }
}
