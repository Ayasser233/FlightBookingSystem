using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlightBookingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Check if user exists
            var existingUser = await userRepository.GetUserByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                return (false, "Email already in use.");
            }

            var newUser = new User
            {
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber,
                Password = createUserDto.Password, // In a real-world app, hash the password
                Role = UserRole.Customer
            };

            await userRepository.Add(newUser);
            return (true, null);
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> ValidateUserCredentialsAsync(LoginDto loginDto)
        {
            var user = await userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || user.Password != loginDto.Password) // Add password hashing and verification in real-world apps
            {
                return (false, "Invalid login credentials.");
            }

            return (true, null);
        }

        public async Task SignInUserAsync(string email)
        {
            var user = await userRepository.GetUserByEmailAsync(email);
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignOutUserAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var email = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userRepository.GetUserByEmailAsync(email);

            return  user;
            
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUserAsync(User updateUserDto)
        {
            var email = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return (false, "User not found.");
            }

            user.FullName = updateUserDto.FullName;
            user.PhoneNumber = updateUserDto.PhoneNumber;
            await userRepository.Update(user);

            return (true, null);
        }
    }
}
