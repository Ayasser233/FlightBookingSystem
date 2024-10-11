using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.ModelView;
using FlightBookingSystem.Repositories;
using FlightBookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FlightBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;
        private readonly AirLineDBcontext context;

        public BookingController(IBookingRepository bookingRepository, IFlightRepository flightRepository, IBookingService bookingService, IPaymentService paymentService
            , AirLineDBcontext _context)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _bookingService = bookingService;
            _paymentService = paymentService;
            context = _context;

        }

        // Step 1: Handle Flight Search Form Submission
        public IActionResult Step1()
        {
            var model = new FlightSearchDto();
            if (model == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Step 1: Flight Search";
            return View(model);
        }



        [HttpPost]
        public IActionResult Step1(int flightId, FlightSearchDto flightSearchDto)
        {
            if (ModelState.IsValid)
            {
                TempData["FlightSearch"] = JsonConvert.SerializeObject(flightSearchDto);
                TempData["SelectedFlightId"] = flightId;
                TempData["Class"] = flightSearchDto.SeatClass;
                TempData.Keep("Class");
                TempData.Keep();
                return RedirectToAction("Step2");
            }
            ViewData["Title"] = "Step 1: Flight Search";
            return View(flightSearchDto);
        }

        // Step 2: Show available flights based on user selection
        [HttpGet]
        public async Task<IActionResult> Step2()
        {
            var flightSearchData = JsonConvert.DeserializeObject<FlightSearchDto>((string)TempData["FlightSearch"]);
            TempData.Keep("FlightSearch");  // Preserve FlightSearch in TempData for subsequent steps
            TempData.Keep("SelectedFlightId");
            var availableFlights = await _flightRepository.GetAvailableFlights(
                flightSearchData.FromAirport, flightSearchData.ToAirport, flightSearchData.FlightDate, flightSearchData.SeatClass);
            
            TempData.Keep("Class");
            return View(availableFlights);
        }
        int FId = 0;
        [HttpPost]
        public async Task<IActionResult> Step2(int flightId)
        {
            FId = flightId;
            if (flightId <= 0)
            {
                ModelState.AddModelError("", "Invalid flight selected.");
                return View(await _flightRepository.GetAllAsync());
            }

            TempData["SelectedFlightId"] = flightId;
            var flight = _flightRepository.GetById(flightId);
            TempData.Keep();
            return RedirectToAction("Step3");
        }

        public IActionResult Step3()
        {
            // Check if TempData contains the necessary data
            if (TempData["FlightSearch"] == null || TempData["SelectedFlightId"] == null)
            {
                return RedirectToAction("Step1");  // Redirect to Step 1 if TempData is lost
            }

            // Preserve the TempData values
            TempData.Keep("FlightSearch");
            TempData.Keep("SelectedFlightId");

            var flightSearchData = JsonConvert.DeserializeObject<FlightSearchDto>((string)TempData["FlightSearch"]);

            var passengers = new List<PassengerDto>();

            for (int i = 0; i < flightSearchData.NumberOfPassengers; i++)
            {
                passengers.Add(new PassengerDto());
            }

            return View(passengers);
        }
        // Step 3: Display Passenger Information Form
        [HttpPost]
        //public async Task<IActionResult> Step3(List<PassengerDto> passengers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Save passengers to the database
        //        foreach (var passengerDto in passengers)
        //        {
        //            var passenger = new Passenger
        //            {
        //                BookingId = (int)TempData["SelectedFlightId"],
        //                FullName = passengerDto.FullName,
        //                DateOfBirth = passengerDto.DateOfBirth,
        //                PassportNumber = passengerDto.PassportNumber,
        //                Nationality = passengerDto.Nationality, // Ensure Nationality is set
        //                SeatClass = (SeatClass)TempData["Class"]
        //            };

        //        }
        //        TempData.Keep("SelectedFlightId");
        //        TempData["Passengers"] = JsonConvert.SerializeObject(passengers);
        //        TempData.Keep();

        //        await context.SaveChangesAsync();

        //        return RedirectToAction("Step4");
        //    }
        //    return View(passengers);
        //}


        [HttpPost]
        public IActionResult Step3(List<PassengerDto> passengers)
        {
            if (ModelState.IsValid)
            {
                TempData.Keep("SelectedFlightId");
                TempData["Passengers"] = JsonConvert.SerializeObject(passengers);
                TempData.Keep();
                return RedirectToAction("Step4");
            }
            return View(passengers);
        }

        // Step 4: Display Payment Page
        [HttpGet]
        public async Task<IActionResult> Step4()
        {
            var flightId = (int)TempData["SelectedFlightId"];
            var flight = await _flightRepository.GetById((int)TempData["SelectedFlightId"]);
            var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>((string)TempData["Passengers"]);

            var paymentDto = new PaymentDto
            {
                Flight = flight,
                Passengers = passengers,
                TotalPrice = flight.BasePrice * passengers.Count
            };
            TempData.Keep("SelectedFlightId");
            TempData["Passengers"] = JsonConvert.SerializeObject(passengers);

            return View(paymentDto);
        }
        //[HttpPost]
        //public async Task<IActionResult> Step4(PaymentDto paymentDto, string FlightData, string PassengersData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return RedirectToAction("Step1");
        //    }

        //    // Deserialize Flight and Passengers from the hidden fields
        //    var flight = JsonConvert.DeserializeObject<Flight>(FlightData);
        //    var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>(PassengersData);

        //    if (flight == null || passengers == null || passengers.Count == 0)
        //    {
        //        return RedirectToAction("Step3");
        //    }

        //    paymentDto.Flight = flight;
        //    paymentDto.Passengers = passengers;

        //    var paymentSuccess = await _paymentService.ProcessPayment(paymentDto);

        //    if (paymentSuccess)
        //    {
        //        await _bookingService.CreateBooking(flight.FlightId, passengers);
        //        return RedirectToAction("Confirmation");
        //    }

        //    ModelState.AddModelError("", "Payment failed");
        //    return View(paymentDto);
        //}


        [HttpPost]
        public async Task<IActionResult> Step4(PaymentDto paymentDto)
        {
            var flightId = (int)TempData["SelectedFlightId"];
            var flight = await _flightRepository.GetById(flightId);
            if (flight == null)
            {
                return RedirectToAction("Step3");
            }

            var passengersJson = (string)TempData["Passengers"];
            if (string.IsNullOrEmpty(passengersJson))
            {
                return RedirectToAction("Step3");
            }

            var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>(passengersJson);
            if (passengers == null || passengers.Count == 0)
            {
                return RedirectToAction("Step3");
            }

            paymentDto.Flight = flight;
            paymentDto.Passengers = passengers;

            TempData.Keep("SelectedFlightId");
            TempData["Passengers"] = JsonConvert.SerializeObject(passengers); 
            TempData["Paymentdto"] = JsonConvert.SerializeObject(paymentDto);
            TempData.Keep();

            if (paymentDto.Passengers.Count != 0 & paymentDto.ExpiryDate.Year >=DateTime.Now.Year)
            {
               

                return RedirectToAction("Register","UserController");
            }

            ModelState.AddModelError("", "Payment failed");
            return View(paymentDto);
        }

        // Confirmation page
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
