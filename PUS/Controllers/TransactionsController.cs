using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS.Data;
using PUS.ViewModels;

namespace PUS.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public TransactionsController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Create(int? serviceID)
        {
            if (serviceID == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == serviceID);

            if (service == null)
            {
                return NotFound();
            }

            var vm = new TransactionCreateViewModel() { ServiceId = service.Id, ServiceTitle = service.Title };

            return PartialView("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreateViewModel vm)
        {




            return Ok();
        }

        public IActionResult TransactionsList()
        {

            return PartialView();
        }
    }
}
