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
            var currentUserId = User.GetUserId();

            if (channelId.HasValue) {
                SelectedChannel = Channels.Single(c => c.Id == channelId.Value);
            } else if (selectedUserId.HasValue) {
                SelectedUser = DirectMessageUsers.Single(u => u.Id == selectedUserId.Value);
            } else {
                SelectedChannel = Channels.First();
            }

            if (IsChannel) {
                MessagesToDisplay = await _context.ChannelMessages
                    .Include(m => m.Sender)
                    .Where(m => m.ChannelId == SelectedChannel.Id)
                    .OrderBy(m => m.CreatedDate)
                    .Cast<Message>()
                    .ToListAsync();
            } else {
                MessagesToDisplay = await _context.DirectMessages
                    .Include(m => m.Sender)
                    .Where(m => m.ReceiverId == currentUserId &&
                                m.SenderId == selectedUserId ||
                                m.ReceiverId == selectedUserId &&
                                m.SenderId == currentUserId)
                    .OrderBy(m => m.CreatedDate)
                    .Cast<Message>()
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? channelId, int? selectedUserId, string newMessage) {
            var currentUserId = User.GetUserId();

            if (string.IsNullOrWhiteSpace(newMessage)) {
                throw new ArgumentException("Message cannot be empty.");
            }

            if (!channelId.HasValue && !selectedUserId.HasValue) {
                throw new ArgumentException("invalid arguments.");
            }

            if (channelId.HasValue) {
                var message = new ChannelMessage {
                    ChannelId = channelId.Value,
                    Content = _messageParsingService.Parse(newMessage),
                    CreatedDate = DateTime.Now,
                    SenderId = User.GetUserId()
                };
                _context.ChannelMessages.Add(message);
            } else {
                var message = new DirectMessage {
                    ReceiverId = selectedUserId.Value,
                    Content = _messageParsingService.Parse(newMessage),
                    CreatedDate = DateTime.Now,
                    SenderId = User.GetUserId()
                };
                _context.DirectMessages.Add(message);
            }
            
            await _context.SaveChangesAsync();

            if (channelId.HasValue) {
                _notificationService.SendUpdateChannelNotification(channelId.Value);
                return RedirectToPage("./Index", new {channelId = channelId});
            }
            else {
                _notificationService.SendDirectMessageNotification(selectedUserId.Value);
                return RedirectToPage("./Index", new {selectedUserId = selectedUserId});
            }
        }
    }
}
