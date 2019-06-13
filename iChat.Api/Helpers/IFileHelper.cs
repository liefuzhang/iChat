using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace iChat.Api.Helpers
{
    public interface IFileHelper
    {
        Task<string> UploadFileAsync(IFormFile file, int workspaceId);
    }
}