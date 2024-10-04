using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlightBookingSystem.Controllers
{

    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return View(bookings);
        }

        // Add booking action
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

        // Edit booking action
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bookingRepository.Update(booking);
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // Get booking action
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
    }
}
