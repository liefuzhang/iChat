using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace iChat.Api.Services
{
    public interface IFileService
    {
        Task<int> UploadFilesAsync(IList<IFormFile> files, int workspaceId);
    }
}