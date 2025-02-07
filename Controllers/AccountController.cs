using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Призначення ролі "Customer" новому користувачеві
                    await _userManager.AddToRoleAsync(user, "Customer");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Model state is valid.");
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    Console.WriteLine($"User found: {user.UserName}");
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        Console.WriteLine("Login successful.");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid login attempt: {result}");
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                else
                {
                    Console.WriteLine("User not found.");
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            else
            {
                Console.WriteLine("Model state is not valid.");
            }
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, "NewPassword123!");
                if (result.Succeeded)
                {
                    Console.WriteLine("Password reset successful.");
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> CheckPasswordHash(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Логування хешу паролю
                Console.WriteLine($"Password hash from database: {user.PasswordHash}");

                var passwordValid = await _userManager.CheckPasswordAsync(user, password);
                if (passwordValid)
                {
                    Console.WriteLine("Password check successful.");
                    return Ok("Password is correct.");
                }
                else
                {
                    Console.WriteLine("Password check failed.");
                    return BadRequest("Invalid password.");
                }
            }
            return NotFound("User not found.");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
