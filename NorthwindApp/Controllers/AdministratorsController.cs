using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NorthwindApp.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AdministratorsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdministratorsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}
