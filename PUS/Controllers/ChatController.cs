using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUS.Data;
using PUS.Models;
using PUS.ViewModels;
using System.Security.Claims;

namespace PUS.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ChatController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int serviceID, string userID)
        {
            var vm = new ChatViewModel();

            var service = await _context.Services.FindAsync(serviceID);
            var user = _context.Profiles.FirstOrDefault(p => p.Id == userID);
            if (service == null || user == null)
            {
                return Json(Status.Unknow);
            }

            var chat = service.Chats.Where(x => x.User == user).FirstOrDefault();
            if (chat == null)
            {
                return Json(Status.Unknow);
            }

            vm.Service = service;
            vm.Chat = chat;

            return PartialView("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(ChatViewModel vm, string message, MessageDirection direction)
        {
            var chat = vm.Service.Chats.FirstOrDefault(x => x.Id == vm.Chat.Id);
            if (chat == null)
            {
                return Json(Status.Unknow);
            }

            var chatLine = new ChatLine() { CreatedAt = DateTime.Now, Direction = direction, Text = message };
            chat.ChatLines.Append(chatLine);

            _context.Update(vm.Service);
            await _context.SaveChangesAsync();

            return Json(Status.Success);
        }

    }
}
