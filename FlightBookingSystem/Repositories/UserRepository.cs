using FlightBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace FlightBookingSystem.Repositories
{
    public class UserRepository : IUserRepository {

        private readonly AirLineDBcontext context ;
        private readonly DbSet<User> users;
        public UserRepository(AirLineDBcontext context)
        {
            this.context = context;
            users = context.Set<User>();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await context.Users.AsQueryable()
        .Include(u => u.Bookings) // Include bookings in the query
        .ThenInclude(b => b.Flight) // Include flight data if needed
        .FirstOrDefaultAsync(u => u.Email == email);
            
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.AsQueryable()
        .Include(u => u.Bookings) // Include bookings in the query
        .ThenInclude(b => b.Flight) // Include flight data if needed
        .FirstOrDefaultAsync(u => u.UserId == id);

        }

        public async Task Add(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await GetById(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }

        
    }

