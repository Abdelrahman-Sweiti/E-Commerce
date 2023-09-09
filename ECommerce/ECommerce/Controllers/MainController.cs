﻿using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.DTOs;
using ECommerce.Models.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace ECommerce.Controllers
{
    public class MainController : Controller
    {

        private readonly IUser _user;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public MainController(IUser user, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _user = user;
            _context = context;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _user.Authenticate(login.Username, login.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                return View("Login", login);
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }
        }



        [HttpGet]
        [Route("Register")]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterUserDTO register)
        {
            var user = await _user.Register(register, this.ModelState);
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", "Main");
            }
            else
            {
                return View("Register", register);

            }

        }

        [Route("Logout")]
        public async Task<IActionResult> LogOut()
        {
            await _user.LogOut();
            return RedirectToAction("Index", "Main");
        }


        [Route("Profile")]
        [HttpGet]
        public IActionResult Profile()
        {


            return View();
        }

        public void SetCookie(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<UserDTO>> Register(RegisterUserDTO registerDTO, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult UpdateInfo()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateInfo(UpdataInfoModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }

            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }

            if (!string.IsNullOrEmpty(model.Gender))
            {
                user.Gender = model.Gender;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Update claims
                var updatedClaims = await _userManager.GetClaimsAsync(user);

                // Sign out the user
                await _signInManager.SignOutAsync();

                // Sign the user in again with the updated claims
                var principal = await _signInManager.CreateUserPrincipalAsync(user);
                await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal);
                var claims = await _userManager.GetClaimsAsync(user);

                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    var firstNameClaim = claims.FirstOrDefault(c => c.Type == "UserFirstName");
                    if (firstNameClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, User.FindFirst("UserFirstName"));
                        await _userManager.AddClaimAsync(user, new Claim("UserFirstName", model.FirstName));
                        await _userManager.ReplaceClaimAsync(user, firstNameClaim, new Claim("UserFirstName", user.FirstName));
                    }
                }

                if (!string.IsNullOrEmpty(model.LastName))
                {
                    var lastNameClaim = claims.FirstOrDefault(c => c.Type == "UserLastName");
                    if (lastNameClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, User.FindFirst("UserLasttName"));
                        await _userManager.AddClaimAsync(user, new Claim("UserLasttName", model.LastName));
                        await _userManager.ReplaceClaimAsync(user, lastNameClaim, new Claim("UserLastName", user.LastName));
                    }
                }

                if (!string.IsNullOrEmpty(model.Gender))
                {
                    var genderClaim = claims.FirstOrDefault(c => c.Type == "Gender");
                    if (genderClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, User.FindFirst("Gender"));
                        await _userManager.AddClaimAsync(user, new Claim("Gender", model.Gender));
                        await _userManager.ReplaceClaimAsync(user, genderClaim, new Claim("Gender", user.Gender));
                    }
                }

                ViewBag.succ = "Update Successful";
                return RedirectToAction("UpdateInfo");
            }
            else
            {
                return View();
            }
        }

    }
}
