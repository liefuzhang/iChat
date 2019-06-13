using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace iChat.Api.Helpers
{
    public interface IFileHelper
    {
        Task<List<string>> UploadFilesAsync(IList<IFormFile> files, int workspaceId);
    }
}