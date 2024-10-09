using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;
using FlightBookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlightBookingSystem.Controllers
{

    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingService _bookingService;
        private readonly IPaymentService _paymentService;

        public BookingController(IBookingRepository bookingRepository, IFlightRepository flightRepository, IBookingService bookingService, IPaymentService paymentService)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _bookingService = bookingService;
            _paymentService = paymentService;
        }



        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return View(bookings);
        }

       
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Booking booking)
        {
            if (ModelState.IsValid)
            {
                await _bookingRepository.Add(booking);
                return RedirectToAction("Index");
            }
            return View(booking);
        }

   

        
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // Delete booking action
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookingRepository.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Step1()
        {
            return View();
        }

        //
        // Step 1: Handle Flight Search Form Submission
        [HttpPost]
        public IActionResult Step1(FlightSearchDto flightSearchDto)
        {
            if (ModelState.IsValid)
            {
                TempData["FlightSearch"] = JsonConvert.SerializeObject(flightSearchDto);
                return RedirectToAction("Step2");
            }
            return View(flightSearchDto);
        }

        // Step 2: Show available flights based on user selection
        [HttpGet]
        public async Task<IActionResult> Step2()
        {
            var flightSearchData = JsonConvert.DeserializeObject<FlightSearchDto>((string)TempData["FlightSearch"]);
            TempData.Keep("FlightSearch");  // Preserve FlightSearch in TempData for subsequent steps

            var availableFlights = await _flightRepository.GetAvailableFlights(
                flightSearchData.FromAirport, flightSearchData.ToAirport, flightSearchData.FlightDate, flightSearchData.SeatClass);

            return View(availableFlights);
        }

        [HttpPost]
        public IActionResult Step2(int selectedFlightId)
        {
            TempData["SelectedFlightId"] = selectedFlightId;
            TempData.Keep("FlightSearch");  // Preserve FlightSearch for Step 3

            return RedirectToAction("Step3");
        }

        // Step 3: Display Passenger Information Form
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


        // Step 3: Handle Passenger Information Submission
        [HttpPost]
        public IActionResult Step3(List<PassengerDto> passengers)
        {
            if (ModelState.IsValid)
            {
                TempData["Passengers"] = JsonConvert.SerializeObject(passengers);
                return RedirectToAction("Step4");
            }
            return View(passengers);
        }

        // Step 4: Display Payment Page
        [HttpGet]
        public IActionResult Step4()
        {
            var flightId = (int)TempData["SelectedFlightId"];
            var flight = _flightRepository.GetById(flightId).Result; // Assuming a repository method to get flight details
            var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>((string)TempData["Passengers"]);

            var paymentDto = new PaymentDto
            {
                Flight = flight,
                Passengers = passengers,
                TotalPrice = flight.BasePrice * passengers.Count
            };

            return View(paymentDto);
        }

        // Step 4: Handle Payment and Complete Booking
        [HttpPost]
        public async Task<IActionResult> CompleteBooking(PaymentDto paymentDto)
        {
            // Check if TempData contains the necessary data
            if (TempData["SelectedFlightId"] == null || TempData["Passengers"] == null)
            {
                return RedirectToAction("Step1");  // Redirect to Step 1 or any other appropriate action
            }

            // Safely retrieve and keep the TempData keys for the next request
            var flightId = (int)TempData["SelectedFlightId"];

            // Deserialize passengers with null check
            var passengersJson = (string)TempData["Passengers"];
            if (string.IsNullOrEmpty(passengersJson))
            {
                return RedirectToAction("Step3"); // Redirect to Step 3 if passengers are not found
            }

            var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>(passengersJson);
            if (passengers == null || passengers.Count == 0)
            {
                return RedirectToAction("Step3"); // Redirect to Step 3 if deserialization fails
            }

            // Validate the paymentDto model
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the same view with the paymentDto
                return View("Step4", paymentDto); // Ensure the view has access to the model
            }

            // Process payment (pseudo-code)
            var paymentSuccess = await _paymentService.ProcessPayment(paymentDto);

            if (paymentSuccess)
            {
                // Save all passenger data and booking
                await _bookingService.CreateBooking(flightId, passengers);

                return RedirectToAction("Confirmation");
            }

            ModelState.AddModelError("", "Payment failed");
            return View("Step4", paymentDto);
        }



        // Confirmation page
        public IActionResult Confirmation()
        {
            return View();
        }

    }
}


