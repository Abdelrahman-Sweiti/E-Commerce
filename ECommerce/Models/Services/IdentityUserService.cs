using ECommerce.Data;
using ECommerce.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models.Services
{
    public class IdentityUserService : IUser
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public IdentityUserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public SignInManager<ApplicationUser> SignInManager { get; }

        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel usermodel)
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
            return result;


        }

        public async Task<SignInResult> PasswordSignInAsync(SignInModel signInModel)
        {

            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, false);
            return result;

        }

        public async Task SignOutAsync()
        {

            await _signInManager.SignOutAsync();
        }


        public int Count()
        {

            return _context.Users.Count();
        }


        public async Task<string> GetUserIdAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user.Id;
        }

       


    }
}
