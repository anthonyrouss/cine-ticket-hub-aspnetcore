using System.Security.Claims;
using CineTicketHub.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CineTicketHub.Models;
using CineTicketHub.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CineTicketHub.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly CineTicketHubContext _context;

        public ReservationsController(CineTicketHubContext context)
        {
            _context = context;
        }

        // GET: Reservations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var isContentManager = User.IsInRole(UserRole.CONTENT_MANAGER.ToString());

            List<Reservation> reservations;
            if (isContentManager)
            {
                reservations = _context.Reservations
                    .Include(r => r.User)
                    .Include(r => r.Screening.Movie)
                    .Include(r => r.Screening.Room)
                    .OrderByDescending(r => r.Screening.StartsAt)
                    .ToList();
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                reservations = _context.Reservations
                    .Where(r => r.UserId == userId)
                    .Include(r => r.Screening.Movie)
                    .Include(r => r.Screening.Room)
                    .OrderByDescending(r => r.Screening.StartsAt)
                    .ToList();
            }
            
            return View(reservations);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "CONTENT_MANAGER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "CONTENT_MANAGER")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
