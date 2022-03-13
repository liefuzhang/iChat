using iChat.Api.Constants;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace iChat.Api.Helpers {
    public class UserIdenticonHelper : IUserIdenticonHelper {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpClientFactory _clientFactory;

        public UserIdenticonHelper(IWebHostEnvironment hostingEnvironment, IHttpClientFactory clientFactory) {
            _hostingEnvironment = hostingEnvironment;
            _clientFactory = clientFactory;
        }

        public async Task<Guid> GenerateUserIdenticon(string email) {
            var identiconGuid = Guid.NewGuid();
            var identiconName = $"{identiconGuid}{iChatConstants.IdenticonExt}";
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, iChatConstants.IdenticonPath,
                identiconName);
            var svgContent = string.Empty;

            try {
                var request = new HttpRequestMessage(HttpMethod.Get,
                        $"https://avatars.dicebear.com/v2/identicon/{identiconName}");

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode) {
                    svgContent = await response.Content.ReadAsStringAsync();
                } else {
                    throw new Exception("Error getting user icon");
                }
            } catch (Exception) {
                var defaultSvgPath = Path.Combine(_hostingEnvironment.WebRootPath, iChatConstants.IdenticonPath,
                    iChatConstants.DefaultIdenticonName);
                svgContent = File.ReadAllText(defaultSvgPath);
            }

            File.WriteAllText(filePath, svgContent);

            return identiconGuid;
        }
    }
}
