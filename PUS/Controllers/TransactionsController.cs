using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS.Data;
using PUS.Models;
using PUS.ViewModels;
using System.Security.Claims;

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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vm = new TransactionCreateViewModel() { ClientId = currentUserId, ServiceId = service.Id, ServiceTitle = service.Title };

            return PartialView("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreateViewModel vm)
        {

            var service = await _context.Services.FirstOrDefaultAsync(m => m.Id == vm.ServiceId);
            var client = _context.Profiles.First(p => p.Id == vm.ClientId);

            if (service == null || client == null)
            {
                return NotFound("serivce or client not found");
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(499, "Model is invalid");
            }

            var transaction = new Transaction()
            {
                OfferBack = vm.OfferBack,
                OfferTo = vm.OfferTo,
                Remarks = vm.Remarks,
                ExchangeDate = vm.ExchangeDate,
                EQI = vm.EQI,
                TransactionStatus = Transaction.Status.Pending,
                //Service = service,
                Client = client
            };
            service.Transactions.Add(transaction);
            _context.Transactions.Add(transaction);
            _context.Update(service);
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult TransactionsList()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var services = _context
                .Services
                .Include(s => s.Owner)
                .Where(s => s.Owner.Id == currentUserId)
                .Include(s => s.Transactions)
                .Include(s => s.Chats)
                .ToList();

            var list = new List<TransactionListViewModel>();
            int i = 1;
            foreach (var service in services)
            {
                foreach (var transaction in service.Transactions)
                {
                    list.Add(
                        new TransactionListViewModel()
                        {
                            ExchangeDate = transaction.ExchangeDate,
                            EQI= transaction.EQI,
                            Position = i++,
                            ServiceId = service.Id,
                            ServiceTitle = service.Title,
                            TransactionId = transaction.Id,
                            TransactionStatus = transaction.TransactionStatus,
                            UserId = transaction.Client.Id,
                            UserName = transaction.Client.UserName
                        }
                        );
                }
            }


            return PartialView("TransactionList", list);
        }
    }
}
