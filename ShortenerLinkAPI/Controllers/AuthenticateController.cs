﻿using Microsoft.AspNetCore.Identity;
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

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInDto request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = await _userManager.FindByNameAsync(request.Mobile);
            var code = Extensions.GenerateSecurityCode();

            if (user is null)
            {
                await _userManager.CreateAsync(new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName= request.Mobile,
                    SecurityCode = code,
                    SecurityCodeExpire = DateTime.Now.AddMinutes(4)
                });
            }
            else
            {
                user.SecurityCode = code;
                user.SecurityCodeExpire = DateTime.Now.AddMinutes(4);
                await _userManager.UpdateAsync(user);
            }

            return Ok($"کد امنیتی {code} ارسال شد");
        }

        [Route("Verify")]
        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyDto request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Mobile && x.SecurityCode == request.Code && x.SecurityCodeExpire.HasValue && x.SecurityCodeExpire.Value >= DateTime.Now);

            if (user is null)
            {
                return BadRequest("کاربری یافت نشد");
            }

            var token = await _accountService.GenerateToken(user.UserName);
            return Ok(token);
        }
    }
}
