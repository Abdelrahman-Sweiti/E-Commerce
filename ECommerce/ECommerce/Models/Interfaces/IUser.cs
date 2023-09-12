using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace ECommerce.Models.Interfaces
{
    public interface IUser
    {

        public Task<UserDTO> Register(RegisterUserDTO registerDto, ModelStateDictionary modelstate, IFormFile file);
        //login Method

        public Task<UserDTO> Authenticate(string username, string password);
        // Get All users method
        public Task<UserDTO> GetUser(ClaimsPrincipal principal);
        // logout method
        public Task LogOut();
        public Task<List<ApplicationUser>> getAll();
        Task<ApplicationUser> GetFile(IFormFile file, ApplicationUser registerUserDTO);


    }
}
