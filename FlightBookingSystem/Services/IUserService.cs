using FlightBookingSystem.DTOs;
using System.Threading.Tasks;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string ErrorMessage)> CreateUserAsync(CreateUserDto createUserDto);
        Task<(bool IsSuccess, string ErrorMessage)> ValidateUserCredentialsAsync(LoginDto loginDto);
        Task SignInUserAsync(string email);
        Task SignOutUserAsync();
        Task<UpdateUserDto> GetCurrentUserAsync();
        Task<(bool IsSuccess, string ErrorMessage)> UpdateUserAsync(UpdateUserDto updateUserDto);
    }
}
