using FlightBookingSystem.Services;
using FlightBookingSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace FlightBookingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // Registration: Display registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Registration: Handle form submission and user creation
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(createUserDto);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Login");
                }

                // Handle error (e.g., user already exists)
                ModelState.AddModelError("", result.ErrorMessage);
            }

            return View(createUserDto);
        }

        // Login: Display login form
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login: Handle login form submission
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.ValidateUserCredentialsAsync(loginDto);

                if (result.IsSuccess)
                {
                    // Sign the user in
                    await userService.SignInUserAsync(loginDto.Email);
                    return RedirectToAction("Profile");
                }

                // Invalid login
                ModelState.AddModelError("", result.ErrorMessage);
            }

            return View(loginDto);
        }

        // Logout: Handle user logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await userService.SignOutUserAsync();
            return RedirectToAction("Login");
        }

        // Profile: Display user profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userService.GetCurrentUserAsync();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // Profile: Edit user profile
        [HttpPost]
        public async Task<IActionResult> EditProfile(UpdateUserDto updateUserDto)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.UpdateUserAsync(updateUserDto);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Profile");
                }

                ModelState.AddModelError("", result.ErrorMessage);
            }

            return View("Profile", updateUserDto);
        }
    }
}
