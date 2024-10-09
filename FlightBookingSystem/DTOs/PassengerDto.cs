using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.DTOs
{
    public class PassengerDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PassportNumber { get; set; }
    }

}
