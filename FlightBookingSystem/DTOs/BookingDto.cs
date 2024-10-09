using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingSystem.DTOs
{
    public class BookingDto
    {
        public int FlightId { get; set; } // ID of the selected flight

        public List<PassengerDto> Passengers { get; set; } // List of passengers

        public decimal TotalPrice { get; set; } // Total price for the booking

        [Required]
        public string PaymentMethod { get; set; } // Selected payment method

        [Required]
        public string CardNumber { get; set; } // Card number for payment

        [Required]
        public DateTime ExpiryDate { get; set; } // Expiry date for payment
    }
}
