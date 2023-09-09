using ECommerce.Data;
using ECommerce.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Services
{
    public class IdentityUserService : IUser
    {

            private UserManager<ApplicationUser> _userManager;
            private SignInManager<ApplicationUser> _signInManager;
            private RoleManager<IdentityRole> _roleManager;

            public IdentityUserService(
                UserManager<ApplicationUser> manager,
                SignInManager<ApplicationUser> signInManager,
                RoleManager<IdentityRole> roleManager)
            {
                _userManager = manager;
                _signInManager = signInManager;
                _roleManager = roleManager;
            }

        public async Task<UserDTO> Register(RegisterUserDTO usermodel, ModelStateDictionary modelState)
        {
            var user = new ApplicationUser()
            {
                FirstName = usermodel.FirstName,
                LastName = usermodel.LastName,
                Gender = usermodel.Gender,
                Image = usermodel.Image,
                Email = usermodel.Email,
                UserName = usermodel.Email
            };

            var result = await _userManager.CreateAsync(user, usermodel.Password);

            if (result.Succeeded)
            {
                // Ensure that the role manager is not null
                if (_roleManager != null)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(usermodel.Roles);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(usermodel.Roles));
                    }

                    await _userManager.AddToRoleAsync(user, usermodel.Roles);
                }

                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }

            foreach (var error in result.Errors)
            {
                var errorKey =
                    error.Code.Contains("Password") ? nameof(usermodel.Password) :
                    error.Code.Contains("Email") ? nameof(usermodel.Email) :
                    error.Code.Contains("UserName") ? nameof(usermodel.Email) :
                    "";

                modelState.AddModelError(errorKey, error.Description);
            }

            return null;
        }

        public async Task<UserDTO> Authenticate(string username, string password)
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, true, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(username);

                    return new UserDTO
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Roles = await _userManager.GetRolesAsync(user)

                    };
                }

                return null;

            }

            public async Task<UserDTO> GetUser(ClaimsPrincipal principal)
            {
                var user = await _userManager.GetUserAsync(principal);
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                };
            }
            public async Task LogOut()
            {
                await _signInManager.SignOutAsync();
            }
            public async Task<List<ApplicationUser>> getAll()
            {
                return await _userManager.Users.ToListAsync();
            }
        }
    }

