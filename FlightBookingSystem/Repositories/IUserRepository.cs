using FlightBookingSystem.Models;
using System.Threading.Tasks;

namespace FlightBookingSystem.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        
    }
}
