using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webmvc.Models;

namespace webmvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PokemonsController> _logger;
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager,
            ILogger<PokemonsController> logger)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View("AccountRegistration");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string passwordhash)
        {
            var account = await _userManager.FindByNameAsync(username);
            if (account != null)
            {
                var result = await _signInManager.PasswordSignInAsync(account, passwordhash, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Pokemons");
                }
            }
            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Pokemons");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string username, string passwordhash)
        {
            var result = await _userManager.CreateAsync(new User { UserName=username}, passwordhash);
            if (!result.Succeeded)
            {
                return Conflict(result.Errors);
            }
            return View("Login");
        }
    }
}
