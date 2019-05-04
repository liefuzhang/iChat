using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Data;
using iChat.Extensions;
using iChat.Models;
using iChat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace iChat.Pages {
    [Authorize]
    public class IndexModel : PageModel {
        private readonly iChatContext _context;
        private readonly INotificationService _notificationService;
        private IMessageParsingService _messageParsingService;

        public IndexModel(iChatContext context,
            INotificationService notificationService,
            IMessageParsingService messageParsingService) {
            _context = context;
            _notificationService = notificationService;
            _messageParsingService = messageParsingService;
        }

        public IList<User> DirectMessageUsers { get; set; }
        public IList<Channel> Channels { get; set; }
        public Channel SelectedChannel { get; set; }
        public User SelectedUser { get; set; }
        public IList<Message> MessagesToDisplay { get; set; }

        public string SelectedName => SelectedChannel?.Name ?? SelectedUser.DisplayName;
        public bool IsChannel => SelectedChannel != null;



        public async Task OnGetAsync(int? channelId, int? selectedUserId) {
            Channels = await _context.Channels.AsNoTracking().ToListAsync();
            DirectMessageUsers = await _context.Users.AsNoTracking().ToListAsync();

            if (channelId.HasValue) {
                SelectedChannel = Channels.Single(c => c.Id == channelId.Value);
            } else if (selectedUserId.HasValue) {
                SelectedUser = DirectMessageUsers.Single(u => u.Id == selectedUserId.Value);
            } else {
                SelectedChannel = Channels.First();
            }

            MessagesToDisplay = await _context.Messages
                .Include(m => m.Sender)
                //.Where(m => (SelectedChannel != null ? 
                //    m.ChannelId == SelectedChannel.Id : 
                //    m.UserId == SelectedUser.Id))
                //.Where(m => m.ChannelId == SelectedChannel.Id)
                .OrderBy(m => m.CreatedDate)
                .ToListAsync();

            var i = 1;
        }

        public async Task<IActionResult> OnPostAsync(int channelId, string newMessage) {
            if (string.IsNullOrWhiteSpace(newMessage)) {
                throw new ArgumentException("Message cannot be empty.");
            }

            if (channelId <= 0) {
                throw new ArgumentException("invalid channel.");
            }

            var message = new ChannelMessage {
                //ChannelId = channelId,
                Content = _messageParsingService.Parse(newMessage),
                CreatedDate = DateTime.Now,
                SenderId = User.GetUserId()
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            _notificationService.SendUpdateChannelNotification(channelId);

            return RedirectToPage("./Index",
                new { channelId = channelId });
        }
    }
}
