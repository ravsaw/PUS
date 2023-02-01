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

        public async Task<IActionResult> Details(int? transactionID)
        {
            if (transactionID == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context
                .Transactions
                .Include(t => t.Service)
                .Include(t => t.Client)
                .FirstOrDefaultAsync(m => m.Id == transactionID);

            if (transaction == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var vm = new TransactionDetailsViewModel() { 
                ServiceTitle = transaction.Service.Title,
                ClientName = transaction.Client.FullName,
                EQI = transaction.EQI,
                ExchangeDate = transaction.ExchangeDate,
                OfferBack = transaction.OfferBack,
                OfferTo = transaction.OfferTo,
                Remarks = transaction.Remarks == "" ? "brak" : transaction.Remarks,
                ServiceId = transaction.Service.Id,
            };

            return PartialView("Details", vm);
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
                return PartialView("Create", vm);
            }

            if (!ModelState.IsValid)
            {
                return PartialView("Create", vm);
            }

            var transaction = new Transaction()
            {
                OfferBack = vm.OfferBack,
                OfferTo = vm.OfferTo,
                Remarks = vm.Remarks ?? "",
                ExchangeDate = vm.ExchangeDate,
                EQI = vm.EQI,
                TransactionStatus = Transaction.Status.Pending,
                Service = service,
                Client = client
            };
            //service.Transactions.Add(transaction);
            _context.Transactions.Add(transaction);
            //_context.Update(service);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public IActionResult TransactionsList()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var services = _context
                .Services
                .Include(s => s.Owner)
                .Where(s => s.Owner.Id == currentUserId)
                .Include("Transactions.Client")
                .Include(s => s.Chats)
                .ToList();

            var tc = _context
                .Transactions
                .Include(t => t.Client)
                .Where(t => t.Client.Id == currentUserId)
                .Include(t => t.Service.Chats)
                .Include("Service.Chats.Client")
                .Include("Service.Transactions.Client")
                .Include(t => t.Service.Owner)
                .ToList();

            var list = new List<TransactionListViewModel>();
            int i = 1;
            foreach (var service in services)
            {

                foreach (var transaction in service.Transactions)
                {
                    var chatId = service.Chats.FirstOrDefault(c => c.Client.Id == transaction.Client.Id)?.Id;
                    if (service.Owner.Id == transaction.Client.Id)
                    {
                        continue;
                    }
                    list.Add(
                        new TransactionListViewModel()
                        {
                            ExchangeDate = transaction.ExchangeDate,
                            EQI = transaction.EQI,
                            Position = i++,
                            ServiceId = service.Id,
                            ServiceTitle = service.Title,
                            TransactionId = transaction.Id,
                            TransactionStatus = transaction.TransactionStatus,
                            ClientId = transaction.Client.Id,
                            OwnerId = transaction.Service.Owner.Id,
                            ClientName = transaction.Client.FullName,
                            OwnerName = transaction.Service.Owner.FullName,
                            UserSide = TransactionListViewModel.Side.provider,
                            ChatId = chatId ?? -1
                        }
                        );
                }

            }

            foreach (var transaction in tc)
            {
                var chatId = transaction.Service.Chats.FirstOrDefault(c => c.Client.Id == transaction.Client.Id)?.Id;
                if(transaction.Service.Owner.Id == transaction.Client.Id)
                {
                    continue;
                }
                list.Add(
                    new TransactionListViewModel()
                    {
                        ExchangeDate = transaction.ExchangeDate,
                        EQI = transaction.EQI,
                        Position = i++,
                        ServiceId = transaction.Service.Id,
                        ServiceTitle = transaction.Service.Title,
                        TransactionId = transaction.Id,
                        TransactionStatus = transaction.TransactionStatus,
                        ClientId = transaction.Client.Id,
                        OwnerId = transaction.Service.Owner.Id,
                        ClientName = transaction.Client.FullName,
                        OwnerName = transaction.Service.Owner.FullName,
                        UserSide = TransactionListViewModel.Side.consumer,
                        ChatId = chatId ?? -1
                    }
                    );
            }

            return PartialView("TransactionsList", list);
        }
    }
}
