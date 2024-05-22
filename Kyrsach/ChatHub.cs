using Kyrsach.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Kyrsach.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Kyrsach
{
    public class Chat : Hub
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SerovaContext _context;
        static List<UserModel> Users = new List<UserModel>();
        public Chat(SerovaContext context, UserManager<UserModel> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        //public async Task SendMessage(string user, string message)
        //{
        //    Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
        [Authorize]
        public async Task SendMessage(string group,string userNmae, string message)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userNmae);
            int idGroup = int.Parse(group);
            var role =  _context.PeopleInGroups.FirstOrDefault(p => p.IdGroup == idGroup && p.IdUser == user.Id).Role;
            var messageEntity = new Message
            {
                IdGroup = idGroup,
                IdSender = user.Id,
                Text = message,
                Time = DateTime.Now
            };
            _context.Messages.Add(messageEntity);
            await _context.SaveChangesAsync();
            await Clients.Group($"Group_{group}").SendAsync("x", user.UserName, $"{user.LastName} {user.FirstName}", message);
        }

        public async Task LoadHistory(string IdGroup)
        {
            //var group =  _context.PeopleInGroups.FirstOrDefault(p => p.IdUser == IdUser).IdGroup;
            if (IdGroup != null)
            {
               int Id = int.Parse(IdGroup);
                var messages = _context.Messages
     .Where(m => m.IdGroup == Id)
     .OrderBy(m => m.Time)
     .Join(_context.Users, m => m.IdSender, u => u.Id, (m, u) => new MessageViewModel
     {
         SenderId = m.IdSender,
         SenderName = $"{u.LastName} {u.FirstName}",
         SenderUserName=u.UserName,
         Role = _context.PeopleInGroups.FirstOrDefault(p => p.IdGroup == Id && p.IdUser == m.IdSender).Role,
         Text = m.Text,
         Time = m.Time.ToString("HH:mm")
     })
     .ToList();

                await Clients.Caller.SendAsync("LoadHistory", messages);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name;
            // Добавить пользователя к группам для его диалогов
            var groups = (from pg in _context.PeopleInGroups
                          join u in _context.Users on pg.IdUser equals u.Id
                          where u.UserName == userId
                          select pg.IdGroup).ToList();

            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Group_{group}");
            }

            await base.OnConnectedAsync();
        }
        //public async Task SendMessage(int group, string sender, string message)
        //{
        //    // Сохраните сообщение в базе данных

        //    Message message1 = new Message
        //    {
        //        Text = message,
        //        IdSender = sender,
        //        Time = DateTime.Now,
        //        IdGroup =group
        //    };
        //    _context.Messages.Add(message1);
        //    await _context.SaveChangesAsync();

        //    // Передайте сообщение через SignalR
        //    await Clients.Group($"Group_{group}").SendAsync("ReceiveMessage", sender, message);
        //}

        //public async Task Messenge(string message, string UserName, int idGroup)
        //{
        //    UserModel user = await _userManager.FindByNameAsync(UserName);
        //    Message messenge = new Message { IdSender = user.Id, Time = DateTime.Now, Text = message, IdGroup= idGroup };
        //    _context.Messages.Add(messenge);
        //    await _context.SaveChangesAsync();
        //    var messenges = _context.Messages.Where(m=>m.IdGroup==idGroup).OrderBy(x => x.Time).ToList();
        //    await Clients.All.SendAsync("getMessenge", messenges);
        //}
        ////public async Task GetUser(string userName)
        ////{
        ////    if (!Users.Any(x => x.UserName == userName))
        ////    {
        ////        User user = await _userManager.FindByNameAsync(userName);
        ////        Users.Add(user);
        ////    }
        ////    await Clients.All.SendAsync("getUsers", Users);
        ////}
        //public override async Task OnConnectedAsync()
        //{
        //    await this.Clients.Caller.SendAsync("getConnected");
        //    await base.OnConnectedAsync();
        //}
        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    User user = Users.FirstOrDefault(x => x.UserName == Context.User.Identity.Name);
        //    Users.Remove(user);
        //    await Clients.All.SendAsync("getUsers", Users);
        //    await base.OnDisconnectedAsync(exception);
        //}



    }
}