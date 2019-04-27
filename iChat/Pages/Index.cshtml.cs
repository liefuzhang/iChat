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

        public async Task OnGetAsync()
        {
            Channels = await _context.Channels.AsNoTracking().ToListAsync();
        }
    }
}
