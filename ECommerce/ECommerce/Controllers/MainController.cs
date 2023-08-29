using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _user.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Main");
                }

                ModelState.AddModelError("", "Invalid information");

            }

            return View();
        }

        [HttpGet]
        [Route("Register")]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel usermodel)
        {
            if (ModelState.IsValid)
            {


                var result = await _user.CreateUserAsync(usermodel);

                if (!result.Succeeded)
                {
                    foreach (var errormessage in result.Errors)
                    {
                        ModelState.AddModelError("", errormessage.Description);
                    }
                    return View(usermodel);

                }


                ModelState.Clear();
            };


            return View();

        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {

            await _user.SignOutAsync();
            return RedirectToAction("Index", "Main");

        }
    }
}
