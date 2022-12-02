using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS.Data;
using PUS.Models;

namespace PUS.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(string id = "")
        {
            if (id == "" || _context.Profiles == null)
            {
                return NotFound();
            }

            var profiles = await _context.Profiles
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (profiles == null)
            {
                return NotFound();
            }

            return PartialView("Details", profiles);
            //return View(service);
        }
    }
}
