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

        [Required]
        [Display(Name = "Card Number")]
        [RegularExpression(@"\d{16}", ErrorMessage = "Please enter a valid 16-digit card number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }
    }
}
