using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlightBookingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // User Management
        public async Task<IActionResult> Users()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // View User Details (instead of Edit)
        [HttpGet]
        public async Task<IActionResult> ViewUser(int id) // Renamed method
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            // Create an UpdateUserDto or similar to display user details
            var updateUserDto = new UpdateUserDto
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
               
            };

            return View(updateUserDto); // Return the view with user details
        }

        // Delete User
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.Delete(id);
            return RedirectToAction("Users");
        }
    }
}
