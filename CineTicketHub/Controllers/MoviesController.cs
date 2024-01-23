using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CineTicketHub.Mappers;
using CineTicketHub.Models.ViewModels;
using CineTicketHub.Services;
using CineTicketHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CineTicketHub.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;   
        private readonly IGenresService _genresService;
        private readonly MovieMapper _movieMapper;
        private readonly CineTicketHubContext _context;

        public MoviesController(IMoviesService service, IGenresService genresService, MovieMapper movieMapper, CineTicketHubContext context)
        {
            _context = context;
            _service = service;
            _genresService = genresService;
            _movieMapper = movieMapper;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _service.GetByIdAsync(id.Value);
            
            if (movie == null)
            {
                return NotFound();
            }
            ViewBag.screenings = _context.Screenings.Include(s => s.Room).Where(s => s.MovieId == id.Value && s.StartsAt >= DateTime.Now).ToList();
            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["GenreIds"] = new SelectList(_genresService.GetAllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieVM movieVM)
        {
            if (!ModelState.IsValid) return View(movieVM);
            await _service.AddMovieAsync(movieVM);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _service.GetByIdAsync(id.Value);
            
            if (movie == null)
            {
                return NotFound();
            }
            
            ViewData["GenreIds"] = new SelectList(_genresService.GetAllAsync().Result, "Id", "Name");
            return View(_movieMapper.MapToMovieVM(movie));
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieVM movieVM)
        {
            if (id != movieVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(movieVM);
            
            await _service.UpdateMovieAsync(movieVM);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _service.GetByIdAsync(id.Value).Result;
            
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
