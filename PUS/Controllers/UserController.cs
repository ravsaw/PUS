using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS.Data;

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
            if (id == "" || _context.Users == null)
            {
                return NotFound();
            }

            var service = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return PartialView("Details", service);
            //return View(service);
        }
    }
}
