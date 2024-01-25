using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CineTicketHub.Models.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CineTicketHub.Models;
using CineTicketHub.Models.ViewModels;
using CineTicketHub.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CineTicketHub.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly CineTicketHubContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly ILogger<ApplicationUsersController> _logger;

        public ApplicationUsersController(UserManager<ApplicationUser> userManager,
            CineTicketHubContext context, ILogger<ApplicationUsersController> logger,
            IUserStore<ApplicationUser> userStore, SignInManager<ApplicationUser> signInManager)
        {   
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
        }

        // GET: ApplicationUser
        public async Task<IActionResult> Index()
        {
            var contentManagers = await _userManager.GetUsersInRoleAsync(UserRole.CONTENT_MANAGER.ToString());

            return View(contentManagers);
        }

        // GET: ApplicationUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email, Password")] UserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);
                // Assign customer role to the user
                await _userManager.AddToRoleAsync(user, UserRole.CONTENT_MANAGER.ToString());
                
                if (result.Succeeded)
                {
                    
                    _logger.LogInformation("User created a new account with password.");

                    return RedirectToAction(nameof(Index));


                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: ApplicationUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: ApplicationUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
    
}
