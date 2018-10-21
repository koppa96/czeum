﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4Server.Areas.Identity.Pages.Account;
using Connect4Server.Data;
using Connect4Server.Models.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Connect4Server.Controllers {
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ILoggerFactory loggerFactory) {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody]AppLoginModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByNameAsync(model.Username);
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded) {
                    _logger.LogInformation(2, "User logged in.");

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Connect4Key"));
                    var token = new JwtSecurityToken();

                    return Ok(token);
                }
            }

            return BadRequest("Error occured");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody]AppRegisterModel model) {
            if (model.Password != model.ConfirmPassword) {
                return BadRequest("The password and the confirmation do not match.");
            }

            if (ModelState.IsValid) {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation(1, "User created new account");

                    //var token = _userManager.CreateSecurityTokenAsync(user);

                    return Ok();
                }
            }

            return BadRequest("Something went wrong.");
        }

        public string GetString() {
            return "Alma";
        }
    }
}