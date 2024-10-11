
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(50), EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public UserRole Role { get; set; } // Enum: Admin, Customer
        [Required, MaxLength(100)] // Adjust max length as necessary
        public string Password { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Customer
    }
}
