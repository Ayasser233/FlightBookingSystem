using FlightBookingSystem.DTOs;
using FlightBookingSystem.Models;
using FlightBookingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlightBookingSystem.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightRepository _flightRepository;

        public FlightController(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<IActionResult> Index()
        {
            var flights = await _flightRepository.GetAllAsync();
            return View(flights);
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Flight flight)
        {
            if (ModelState.IsValid)
            {
                flight.AvailableSeats = flight.TotalSeats - flight.BookedSeats;
                await _flightRepository.Add(flight);
                return RedirectToAction("Index");
            }
            return View(flight);
        }

      

        public async Task<IActionResult> Edit(int id)
        {
            var flight = await _flightRepository.GetById(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit";
            return View(flight);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , Flight flight)
        {
            if (ModelState.IsValid & id == flight.FlightId)
            {
                await _flightRepository.Update(flight);
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        public async Task<IActionResult> Details(int id)
        {
            var flight = await _flightRepository.GetById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _flightRepository.GetById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _flightRepository.Delete(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Step2()
        {
            var flightSearchData = JsonConvert.DeserializeObject<FlightSearchDto>((string)TempData["FlightSearch"]);
            var availableFlights = await _flightRepository.GetAvailableFlights(flightSearchData.FromAirport, flightSearchData.ToAirport, flightSearchData.FlightDate, flightSearchData.SeatClass);

            return View(availableFlights);
        }


    }
}
