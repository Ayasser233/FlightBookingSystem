using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using System.Threading.Tasks;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string ErrorMessage)> CreateUserAsync(CreateUserDto createUserDto);
        Task<(bool IsSuccess, string ErrorMessage)> ValidateUserCredentialsAsync(LoginDto loginDto);
        Task SignInUserAsync(string email);
        Task SignOutUserAsync();
        Task<User> GetCurrentUserAsync();
        Task<(bool IsSuccess, string ErrorMessage)> UpdateUserAsync(User updateUserDto);
        
    }
}
