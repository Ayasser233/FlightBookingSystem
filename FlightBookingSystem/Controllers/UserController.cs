using FlightBookingSystem.Services;
using FlightBookingSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using FlightBookingSystem.Models;
using Newtonsoft.Json;
using FlightBookingSystem.Repositories;
using System.Net;

namespace FlightBookingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IFlightRepository flightRepository;
        private readonly IUserRepository userRepository;
        private readonly IBookingService bookingService;
        private readonly AirLineDBcontext airLineD;

        public UserController(IUserService userService, IFlightRepository flightRepository, IUserRepository userRepository, IBookingService bookingService, AirLineDBcontext airLineDBcontext)
        {
            this.userService = userService;
            this.flightRepository = flightRepository;
            this.userRepository = userRepository;
            this.bookingService = bookingService;
            airLineD = airLineDBcontext;
        }

        // Registration: Display registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Registration: Handle form submission and user creation
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            User us = await userRepository.GetUserByEmailAsync(createUserDto.Email);
            if (us != null)
            {
                return RedirectToAction("Login", "User");
            }
            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(createUserDto);

                if (result.IsSuccess)
                {
                    var flightId = (int)TempData["SelectedFlightId"];
                    var passengersJson = (string)TempData["Passengers"];
                    var paymentJson = (string)TempData["Paymentdto"];
                    IEnumerable<User> u = await userRepository.GetAllAsync();
                    var userid = u.LastOrDefault()?.UserId ?? 0;
                    var flight = await flightRepository.GetById(flightId);
                    var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>(passengersJson);
                    var payment = JsonConvert.DeserializeObject<PaymentDto>(paymentJson);
                    await bookingService.CreateBooking(userid, flight.FlightId, passengers, payment);
                    var Bookid = (int)airLineD.Bookings.OrderBy(b => b.BookingId).LastOrDefault()?.BookingId;
                    var paymentDb = new Payment
                    {
                        BookingId = Bookid,
                        Amount = payment.TotalPrice,
                        PaymentMethod = payment.PaymentMethod,
                        PaymentDate = payment.ExpiryDate
                    };
                    airLineD.Payments.AddAsync(paymentDb);
                    airLineD.SaveChangesAsync();
                    TempData["BookingId"] = Bookid;
                    return RedirectToAction("Profile");
                }

                // Handle error (e.g., user already exists)
                ModelState.AddModelError("", result.ErrorMessage);
            }
        
            return View(createUserDto);
        } 

        // Login: Display login form
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login: Handle login form submission
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.ValidateUserCredentialsAsync(loginDto);

                if (result.IsSuccess)
                {
                    if (TempData.ContainsKey("SelectedFlightId") && TempData.ContainsKey("Passengers") && TempData.ContainsKey("Paymentdto"))
                    {
                        var flightId = (int)TempData["SelectedFlightId"];
                        var passengersJson = (string)TempData["Passengers"];
                        var paymentJson = (string)TempData["Paymentdto"];
                        User u = await userRepository.GetUserByEmailAsync(loginDto.Email);
                        var userid = u.UserId;
                        var flight = await flightRepository.GetById(flightId);
                        var passengers = JsonConvert.DeserializeObject<List<PassengerDto>>(passengersJson);
                        var payment = JsonConvert.DeserializeObject<PaymentDto>(paymentJson);
                        await bookingService.CreateBooking(userid, flight.FlightId, passengers, payment);
                        await userService.SignInUserAsync(loginDto.Email);
                        var Bookid = (int)airLineD.Bookings.OrderBy(b => b.BookingId).LastOrDefault()?.BookingId;

                        var paymentDb = new Payment
                        {
                            BookingId = Bookid,
                            Amount = payment.TotalPrice,
                            PaymentMethod = payment.PaymentMethod,
                            PaymentDate = payment.ExpiryDate
                        };
                        airLineD.Payments.AddAsync(paymentDb);
                        airLineD.SaveChangesAsync();
                        User userDb = await userRepository.GetUserByEmailAsync(loginDto.Email);
                        var booking = userDb.Bookings;
                        TempData["Bookings"] = booking;
                        return RedirectToAction("Profile" , userDb);
                    }
                    else
                    {
                        User userDb = await userRepository.GetUserByEmailAsync(loginDto.Email);
                        var booking = userDb.Bookings;
                        TempData["Bookings"] = booking;
                        return RedirectToAction("Profile");
                    }
                }
                ModelState.AddModelError("", result.ErrorMessage);
            }
            return View(loginDto);
        }

        // Logout: Handle user logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await userService.SignOutUserAsync();
            return RedirectToAction("Login");
        }

            // Profile: Display user profile
            [HttpGet]
            public async Task<IActionResult> Profile()
            {
                var user = await userService.GetCurrentUserAsync();

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                return View(user);
            }

            // Profile: Edit user profile
            [HttpPost]
            public async Task<IActionResult> EditProfile(UpdateUserDto updateUserDto)
            {
                if (ModelState.IsValid)
                {
                    var result = await userService.UpdateUserAsync(updateUserDto);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Profile");
                    }

                    ModelState.AddModelError("", result.ErrorMessage);
                }

                return View("Profile", updateUserDto);
            }
        }
    }

