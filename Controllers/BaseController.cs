using System.Security.Claims;
using MenuBackend.Models.Auth;
using MenuBackend.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MenuBackend.Controllers
{


    public class BaseController : ControllerBase
    {

        protected readonly UserManager<ApplicationUser>? userManager;
        protected readonly ApplicationDbContext dbContext;

        public BaseController(
           UserManager<ApplicationUser>? _userManager,
       ApplicationDbContext _dbContext
     )
        {
            if (_userManager != null)
            {
                this.userManager = _userManager;
            }
            this.dbContext = _dbContext;
        }

        protected async Task<ApplicationUser?> GetUserFromRequest()
        {
            if (HttpContext.User.FindFirstValue("UserId") != null)
            {
                var idClaim = HttpContext.User.FindFirstValue("UserId");
                if (userManager != null)
                {
                    var user = await userManager.FindByIdAsync(idClaim!);
                    return user;
                }
            }
            return null;
        }

    }


}
