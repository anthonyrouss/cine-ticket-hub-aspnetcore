using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineTicketHub.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CineTicketHub.Models;
using CineTicketHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CineTicketHub.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly CineTicketHubContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScreeningsController(CineTicketHubContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Screenings
        public async Task<IActionResult> Index()
        {
            var cineTicketHubContext = _context.Screenings.Include(s => s.Movie).Include(s => s.Room);
            return View(await cineTicketHubContext.ToListAsync());
        }

        // GET: Screenings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // GET: Screenings/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id");
            return View();
        }

        // POST: Screenings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,RoomId,StartsAt")] Screening screening)
        {
            // Ignore the movie and room properties
            ModelState.Remove(nameof(Screening.Movie));
            ModelState.Remove(nameof(Screening.Room));
            
            // Check if the movie id exists
            if (!MovieExists(screening.MovieId))
                ModelState.AddModelError(nameof(Screening.MovieId), $"Movie with ID {screening.MovieId} does not exist.");
            
            // Check if the room id exists
            if (!RoomExists(screening.RoomId))
                ModelState.AddModelError(nameof(Screening.RoomId), $"Room with ID {screening.RoomId} does not exist.");
            
            if (!ModelState.IsValid)
            {
                ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", screening.MovieId);
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", screening.RoomId);
                return View(screening);
            }
            
            _context.Add(screening);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Screenings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", screening.MovieId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", screening.RoomId);
            return View(screening);
        }

        // POST: Screenings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,RoomId,StartsAt")] Screening screening)
        {
            if (id != screening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", screening.MovieId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", screening.RoomId);
            return View(screening);
        }

        // GET: Screenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // POST: Screenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);
            if (screening != null)
            {
                _context.Screenings.Remove(screening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Screenings/{screeningId}/reservations
        [HttpGet]
        [Route("/screenings/{screeningId}/reservations/create")]
        [Authorize]
        public async Task<ViewResult> CreateReservation(int screeningId)
        {
            Reservation newReservation = new Reservation()
            {
                Screening = await _context.Screenings
                    .Include(s => s.Movie)
                    .Include(s => s.Room)
                    .FirstOrDefaultAsync(s => s.Id == screeningId)
            };
            ViewBag.SeatsLeft = seatsLeft(screeningId);
            return View(newReservation);
        }
        
        // GET: Screenings/{screeningId}/reservations
        [HttpPost]
        [Route("/screenings/{screeningId}/reservations/create")]
        [Authorize]
        public async Task<ActionResult> CreateReservation(int screeningId, [Bind("Screening, NumOfSeats")] Reservation reservation)
        {
            
            // Ignore the movie and room properties
            ModelState.Remove(nameof(Reservation.User));
            ModelState.Remove(nameof(Reservation.UserId));
            ModelState.Remove(nameof(Reservation.Screening));
            
            if (reservation.NumOfSeats <= 0)
                ModelState.AddModelError(nameof(Reservation.NumOfSeats), $"Invalid number of seats.");

            var availableSeats = seatsLeft(screeningId);
            
            if (reservation.NumOfSeats > availableSeats)
                ModelState.AddModelError(nameof(Reservation.NumOfSeats), $"Only {availableSeats} seats are left.");

            Screening currentScreening = await _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == screeningId);
            
            if (!ModelState.IsValid)
            {
                ViewBag.SeatsLeft = seatsLeft(screeningId);
                return View(new Reservation() { Screening = currentScreening });
            }
            
            var newReservation = new Reservation()
            {
                ScreeningId = screeningId,
                Screening = currentScreening,
                UserId = _userManager.GetUserId(User),
                NumOfSeats = reservation.NumOfSeats
            };
                
            await _context.Reservations.AddAsync(newReservation);
            await _context.SaveChangesAsync();
            
            // Pass the reservation model to the confirmation view
            return View("ConfirmReservation", newReservation);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(m => m.Id == id);
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(r => r.Id == id);
        }
        
        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }

        private int seatsLeft(int screeningId)
        {
            var roomCap = _context.Screenings
                .Include(s => s.Room)
                .FirstOrDefault(s => s.Id == screeningId)
                .Room.Capacity;

            var totalReservedSeats = _context.Reservations
                .Where(r => r.ScreeningId == screeningId)
                .Sum(r => r.NumOfSeats);
            
            return roomCap - totalReservedSeats;
        }
    }
}
