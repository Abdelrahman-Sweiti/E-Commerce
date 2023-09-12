using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models.Interfaces
{
    public interface IUser
    {

        Task<IdentityResult> CreateUserAsync(RegisterViewModel usermodel);
        Task<SignInResult> PasswordSignInAsync(SignInModel signInModel);
        Task SignOutAsync();
    }
}
