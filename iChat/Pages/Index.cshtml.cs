using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Data;
using iChat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace iChat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly iChatContext _context;

        public IndexModel(iChatContext context) {
            _context = context;
        }

        public IList<Channel> Channels { get; set; }
        public Channel SelectedChannel { get; set; }

        public async Task OnGetAsync(int? channelID)
        {
            Channels = await _context.Channels.AsNoTracking().ToListAsync();
            if (channelID.HasValue) {
                SelectedChannel = Channels.Single(c => c.ID == channelID.Value);
            }
            else {
                SelectedChannel = Channels.First();
            }
        }
    }
}
