using FeedApi.Model;
using FeedApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Controllers
{
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _loggerReg;
        private readonly ILogger<LoginModel> _loggerLogin;
        private readonly ITokenService _tokenService;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> loggerLog,
                 UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> loggerReg,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerLogin = loggerLog;
            _loggerReg = loggerReg;
            _tokenService = tokenService;
        }

        [AllowAnonymous]

        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                   var token =  _tokenService.GenerateToken(model.Email, TimeSpan.FromDays(1));
                    _loggerLogin.LogInformation("User logged in.");
                    return Ok(token);
                }

                if (result.IsLockedOut)
                {
                    _loggerLogin.LogWarning("User account locked out.");
                    return StatusCode(423);
                }
            }
            return new JsonResult("Invalid login attempt");
        }

        [Route("/token")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            var token = _tokenService.GenerateToken(User.Identity.Name, TimeSpan.FromDays(1));
                    _loggerLogin.LogInformation("User refresh token.");
                    return Ok(token);                
        }

        [AllowAnonymous]

        [Route("/registration")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _loggerReg.LogInformation("User created a new account with password.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                   var token =  _tokenService.GenerateToken(model.Email, TimeSpan.FromDays(1));
                    return Ok(token);
                }
                return BadRequest(result.Errors.Select(x => x.Description).ToList());
            }
            return BadRequest(ModelState.Select(x => x.Value.Errors.ToList()).ToList());
        }

    }
}