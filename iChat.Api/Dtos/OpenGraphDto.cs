using iChat.Api.Models;
using System.Collections.Generic;

namespace iChat.Api.Dtos {
    public class OpenGraphDto
    {
        public string Url { get; set; }
        public string SiteName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}