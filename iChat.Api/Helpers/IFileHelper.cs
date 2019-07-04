using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace iChat.Api.Helpers
{
    public interface IFileHelper
    {
        Task<string> UploadFileAsync(IFormFile file, int workspaceId);
        Task<Stream> DownloadFileAsync(string savedName, int workspaceId);
        Task DeleteFileAsync(string savedName, int workspaceId);
    }
}