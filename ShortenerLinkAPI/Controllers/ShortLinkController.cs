using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShortLinkGenerator.Data;
using ShortLinkGenerator.Helpers;
using ShortLinkGenerator.Models;
using ShortLinkGenerator.Services;
using ShortLinkGenerator.ViewModels;

using System;
using System.Net;

namespace ShortLinkGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ShortLinkController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;
        public ShortLinkController(ApplicationDbContext context, IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _accountService = accountService;
            _userManager = userManager;
        }

        [Route("CreateShortLink")]
        [HttpPost]
        public async Task<IActionResult> CreateShortLink(ShortLinkRequestDto request)
        {
            try
            {
                var userClaim = await _accountService.GetUserFromClaim(claimsPrincipal: User);
                var thisUser = await _userManager.FindByNameAsync(userClaim.Username);

                if (thisUser is null)
                {
                    return Unauthorized();
                }

                string decodedUrl = WebUtility.UrlDecode(request.Url);


                var link = await _context.Links.FirstOrDefaultAsync(x => x.OriginalUrl == decodedUrl);
               

                if (link is not null)
                {
                    var response = new ShortLinkResponseDto()
                    {
                        Status=false,
                        Message = "Your link has already been created!",
                        Url = link.Url
                    };

                    return Ok(response);
                }
                else
                {
                    var key = Extensions.GenerateLinkShortKey();
                    while (await _context.Links.AnyAsync(x => x.Key == key))
                    {
                        key = Extensions.GenerateLinkShortKey();
                    }
                    var url = $"{Request.Scheme}://{Request.Host}/{key}";

                    await _context.Links.AddAsync(new Link()
                    {
                        OriginalUrl = decodedUrl,
                        Url = url,
                        Key = key,
                        UserId = thisUser.Id,
                        CreateDate = DateTime.Now
                    });
                    await _context.SaveChangesAsync();

                    var response = new ShortLinkResponseDto()
                    {
                        Status=true,
                        Message = "Your link was created successfully.",
                        Url = url
                    };

                    return Ok(response);
                }
            }
            catch (Exception err)
            {

                throw;
            }

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("/{shortKey}")]
        public async Task<IActionResult> RedirectShortLink(string shortKey)
        {
            var link = await _context.Links.FirstOrDefaultAsync(x => x.Key == shortKey);
            if (link is null)
            {
                return NotFound();
            }
            return Redirect(link.OriginalUrl);
        }
    }
}
