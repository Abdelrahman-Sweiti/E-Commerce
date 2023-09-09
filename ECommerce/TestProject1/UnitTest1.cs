using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerce.Models;
using ECommerce.Models.DTOs;
using ECommerce.Models.Interfaces;
using ECommerce.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;

namespace TestProject1
{
    public class IdentityUserServiceTests
    {
        [Fact]
        public async Task Register_ValidUser_ReturnsUserDTO()
        {
            // Arrange
            var userManager = MockUserManager();
            var signInManager = MockSignInManager(); // Add this line
            var roleManager = MockRoleManager();
            var userService = new IdentityUserService(userManager.Object, signInManager.Object, roleManager.Object); // Pass signInManager here
            var usermodel = new RegisterUserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Image = "profile.jpg",
                Email = "john@example.com",
                Password = "P@ssw0rd",
                Roles = "UserRole"
            };

            // Act
            var result = await userService.Register(usermodel, new ModelStateDictionary());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result.Username);
            Assert.Contains("UserRole", result.Roles);
        }


        [Fact]
        public async Task Authenticate_ValidUser_ReturnsUserDTO()
        {
            // Arrange
            var userManager = MockUserManager();
            var signInManager = MockSignInManager();
            var roleManager = MockRoleManager();
            var userService = new IdentityUserService( userManager.Object, signInManager.Object, roleManager.Object);

            var user = new ApplicationUser
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
                // Add any other properties as needed
            };

            userManager.Setup(u => u.FindByNameAsync("testuser")).ReturnsAsync(user);

            var login = new LoginDTO
            {
                Username = "testuser",
                Password = "P@ssw0rd"
            };

            signInManager.Setup(sm => sm.PasswordSignInAsync("testuser", "P@ssw0rd", true, false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await userService.Authenticate(login.Username, login.Password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("testuser", result.Username);
            // Add assertions for roles if needed
        }


        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { UserName = "john@example.com" });

            userManager.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "UserRole" });

            return userManager;
        }

        private Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
            roleManager.Setup(r => r.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            return roleManager;
        }

        private Mock<SignInManager<ApplicationUser>> MockSignInManager()
        {
            var userManager = MockUserManager();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
            signInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), true, false))
                .ReturnsAsync(SignInResult.Success);

            return signInManager;
        }
    }
}
