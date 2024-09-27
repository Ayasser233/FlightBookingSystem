using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.DTOs
{
    public class UpdateUserDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }
    }
}
