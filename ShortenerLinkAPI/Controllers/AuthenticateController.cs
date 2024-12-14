using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShortLinkGenerator.Data;
using ShortLinkGenerator.Helpers;
using ShortLinkGenerator.Models;
using ShortLinkGenerator.Services;
using ShortLinkGenerator.ViewModels;

namespace ShortLinkGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;
        public AuthenticateController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAccountService accountService)
        {
            _context = context;
            _userManager = userManager;
            _accountService = accountService;
        }
        [Route("GetTest")]
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("test");
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInDto request)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                var user = await _userManager.FindByNameAsync(request.Mobile);
                var code = Extensions.GenerateSecurityCode();

                if (user is null)
                {
                    await _userManager.CreateAsync(new ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = request.Mobile,
                        SecurityCode = code,
                        SecurityCodeExpire = DateTime.Now.AddMinutes(1)
                    });
                }
                else
                {
                    user.SecurityCode = code;
                    user.SecurityCodeExpire = DateTime.Now.AddMinutes(1);
                    await _userManager.UpdateAsync(user);
                }

                return Ok(new ApiOkResponse($"Security Code Send ! {code}"));
            }
            catch (Exception err)
            {
                return BadRequest(err.Message + err.InnerException.Message);
            }
        }

        [Route("Verify")]
        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyDto request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Mobile);

            if (user is null)
            {
                return BadRequest(new ApiResponse(400, "Not Found User !"));
            }
            if (user.SecurityCode != request.Code)
            {
                return BadRequest(new ApiResponse(400, "Incorrect Security Code !"));
            }

            if (!user.SecurityCodeExpire.HasValue || user.SecurityCodeExpire.Value < DateTime.Now)
            {
                return BadRequest(new ApiResponse(400, "Security Code Expired !"));
            }
            var token = await _accountService.GenerateToken(user.UserName);
            return Ok(token);
        }
    }
}
