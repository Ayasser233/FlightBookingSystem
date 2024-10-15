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
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingService bookingService;
        private readonly AirLineDBcontext airLineD;

        public UserController(IUserService userService, IFlightRepository flightRepository, IUserRepository userRepository, IBookingService bookingService, AirLineDBcontext airLineDBcontext,IBookingRepository repository)
        {
            this.userService = userService;
            this.flightRepository = flightRepository;
            this.userRepository = userRepository;
            this.bookingService = bookingService;
            airLineD = airLineDBcontext;
            bookingRepository = repository;
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
                    IEnumerable<User> use = await userRepository.GetAllAsync();
                    User userdb = use.LastOrDefault();
                    Booking b = airLineD.Bookings.FirstOrDefault(b => b.BookingId == Bookid);
                    b.Status = BookingStatus.Confirmed;
                    userdb.Bookings.Add(b); // Add a new Booking object
                    airLineD.Users.Update(userdb);
                    airLineD.SaveChanges();
                    User userDb1 = await userRepository.GetUserByEmailAsync(createUserDto.Email);
                    TempData["BookingId"] = Bookid;
                    return RedirectToAction("Profile" , userDb1);
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
                        await airLineD.Payments.AddAsync(paymentDb);
                        await airLineD.SaveChangesAsync();
                        User userDb = await userRepository.GetUserByEmailAsync(loginDto.Email);
                        Booking b = airLineD.Bookings.FirstOrDefault(b=>b.BookingId== Bookid);
                        b.Status = BookingStatus.Confirmed;
                        userDb.Bookings.Add(b ); // Add a new Booking object
                        airLineD.Users.Update(userDb);
                        await airLineD.SaveChangesAsync();
                        var booking = userDb.Bookings;
                        User userDb1 = await userRepository.GetUserByEmailAsync(loginDto.Email);


                        return RedirectToAction("Profile", userDb1);
                    }
                    else
                    {
                        User userDb = await userRepository.GetUserByEmailAsync(loginDto.Email);
                        var booking = userDb.Bookings;

                        return RedirectToAction("Profile", userDb);
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
        public async Task<IActionResult> Profile(int userId)
        {
            // Retrieve the user data from the database using the user ID
            User user = await userRepository.GetById(userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }


        // Profile: Edit user profile
        [HttpPost]
        public async Task<IActionResult> EditProfile(User updateUserDto)
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

            var user = new User
            {
                FullName = updateUserDto.FullName,
                PhoneNumber = updateUserDto.PhoneNumber,
                
            };

            return View("Profile", user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound(); // 404 if the booking doesn't exist
            }
            return View(booking);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // ActionName "Delete" will map this to the form action
        {
            var booking = await bookingRepository.GetById(id); // Ensure the booking is fetched before deletion
            if (booking == null)
            {
                return NotFound();
            }
            await bookingRepository.Delete(id); // Perform deletion from the database
            await airLineD.SaveChangesAsync(); // Save changes to the database
            return RedirectToAction("Profile", new { userId = booking.UserId }); // Redirect to profile after deletion
        }



    }
}

