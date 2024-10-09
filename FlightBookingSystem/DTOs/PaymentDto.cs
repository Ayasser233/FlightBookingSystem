using FlightBookingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.DTOs
{
    public class PaymentDto
    {
        public Flight Flight { get; set; }

        public List<PassengerDto> Passengers { get; set; }

        public decimal TotalPrice { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
        public string? CardNumber { get; internal set; }
        public DateTime ExpiryDate { get; internal set; }
    }

}
