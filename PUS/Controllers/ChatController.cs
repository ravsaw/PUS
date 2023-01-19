﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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

        [HttpGet]
        public async Task<IActionResult> Index(int serviceID, string? userID = "")
        {
            var vm = new ChatViewModel();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.Profiles.First(p => p.Id == currentUserId);
            vm.currentUserID = currentUserId;

            Profile? user;
            if (userID == "")
            {
                user = currentUser;
            }
            else
            {
                user = _context.Profiles.FirstOrDefault(p => p.Id == userID);
            }

            var service = await _context.Services
                        .Where(s => s.Id == serviceID)
                        .Include("Chats.ChatLines")
                        .FirstOrDefaultAsync();

            if (service == null || user == null)
            {
                return Json(Status.Unknow);
            }

            var chat = service.Chats.Where(x => x.Client == user).FirstOrDefault();
            if (chat == null)
            {
                chat = new Chat() { Client = user, ChatLines = new List<ChatLine>() };
                service.Chats.Add(chat);
                _context.Add(chat);
                _context.Update(service);
                await _context.SaveChangesAsync();
            }

            vm.serviceID = service.Id;
            vm.serviceTitle = service.Title;
            vm.chatID = chat.Id;
            vm.chatLines = chat.ChatLines.ToList();
            vm.chatLines.Reverse();

            if (chat.Client != currentUser && service.Owner != currentUser)
            {
                return Json(Status.Unknow);
            }

            return PartialView("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(ChatViewModel vm, string message)
        {

            var service = await _context.Services
                .Include("Chats.ChatLines")
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.Id == vm.serviceID);

            if (service == null)
            {
                return Json(Status.Unknow);
            }

            var chat = service.Chats.FirstOrDefault(x => x.Id == vm.chatID);
            if (chat == null)
            {
                return Json(Status.Unknow);
            }

            MessageDirection direction;
            if (service.Owner.Id == vm.currentUserID)
            {
                direction = MessageDirection.From;
            }
            else
            {
                direction = MessageDirection.To;
            }

            var chatLine = new ChatLine() { CreatedAt = DateTime.Now, Direction = direction, Text = message };
            _context.Add(chatLine);
            chat.ChatLines.Add(chatLine);
            _context.Update(chat);
            await _context.SaveChangesAsync();

            vm.chatLines = chat.ChatLines.ToList();
            vm.chatLines.Reverse();

            return PartialView("Index", vm);
        }


        [HttpGet]
        public IActionResult ChatList()
        {


            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.Profiles.First(p => p.Id == currentUserId);


            var query = _context.Services
                .Include(s => s.Owner)
                .Where(s => s.Owner.Id == currentUserId)
                .Join(
                    _context.Chats,
                    service => service.Id,
                    chat => chat.Service.Id,
                    (service, chat) => new
                    {
                        service,
                        chat
                    }
                )
                .Join(
                    _context.Profiles,
                    sc => sc.chat.Client.Id,
                    profile => profile.Id,
                    (sc, profile) => new
                    {
                        sc.chat.LastUpdate,
                        ServiceTitle = sc.service.Title,
                        ServiceId = sc.service.Id,
                        UserName = profile.FirstName + " " + profile.LastName,
                        UserId = profile.Id,
                        ChatId = sc.chat.Id
                    }
                )
                .OrderBy(q => q.LastUpdate);

            int i = 0;
            var list = new List<ChatListViewModel>();


            foreach (var item in query)
            {
                var vm = new ChatListViewModel()
                {
                    Position = i++,
                    LastUpdate = item.LastUpdate,
                    ServiceTitle = item.ServiceTitle,
                    User = item.UserName,
                    UserId = item.UserId,
                    TransactionStatus = ChatListViewModel.Status.Pending,
                    ServiceId = item.ServiceId,
                    ChatId = item.ChatId
                };
                list.Add(vm);
            }

            return PartialView("ChatList", list);
        }
    }
}
