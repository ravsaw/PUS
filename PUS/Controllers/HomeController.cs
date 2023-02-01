using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS.Data;
using PUS.Models;
using PUS.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace PUS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id == currentUserId);

            if(user != null)
            {
                TempData["FullName"] = user.FullName;
            } else
            {
                TempData["FullName"] = "";
            }


            var dateNow = DateTime.Now;

            var services = await _context.Services
                .Where(s => s.StartDate < dateNow)
                .Where(s => s.EndDate > dateNow)
                .Where(s => !s.IsArchived)
                .OrderBy(s => s.StartDate)
                .Include(s => s.Owner)
                .Where(s => s.Owner.Id != currentUserId)
                .ToListAsync();

            var vm = new List<HomeViewModel>();
            foreach (var service in services)
            {
                vm.Add(new HomeViewModel()
                {
                    ServiceId = service.Id,
                    ServiceTitle = service.Title,
                    OwnerId = service.Owner.Id,
                    OwnerGoodLevel = service.Owner.GoodLevel,
                    OwnerNeutralLevel = service.Owner.NeutralLevel,
                    OwnerBadLevel = service.Owner.BadLevel
                });
            }

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}