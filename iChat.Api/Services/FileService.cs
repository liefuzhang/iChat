using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using iChat.Api.Constants;
using iChat.Api.Data;
using iChat.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Services
{
    public class FileService : IFileService
    {
        private readonly iChatContext _context;
        private readonly AppSettings _appSettings;

        public FileService(iChatContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        // TODO refactor out FileHelper
        // create file table, message file table
        public async Task<int> UploadFilesAsync(IList<IFormFile> files, int workspaceId)
        {
            var uploadToSubFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;
            foreach (var file in files)
            {
                var fileName = file.FileName;
                while (await FileExistsAsync(fileName, workspaceId))
                {
                    fileName = Guid.NewGuid() + file.FileName;
                }

                using (var client = new AmazonS3Client(_appSettings.AwsAccessKeyId, _appSettings.AwsSecretAccessKey, RegionEndpoint.APSoutheast2))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);

                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = memoryStream,
                            Key = fileName,
                            BucketName = $"{_appSettings.AwsFileBucketName}/{uploadToSubFolder}",
                            CannedACL = S3CannedACL.PublicRead // TODO change to private
                        };

                        var fileTransferUtility = new TransferUtility(client);
                        await fileTransferUtility.UploadAsync(uploadRequest);
                    }
                }
            }

            return 0;
        }

        private async Task<bool> FileExistsAsync(string fileName, int workspaceId)
        {
            var subFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;

            using (var client = new AmazonS3Client(RegionEndpoint.APSoutheast2))
            {
                var exist = false;
                try
                {
                    await client.GetObjectMetadataAsync(_appSettings.AwsFileBucketName, $"{subFolder}/{fileName}");
                    exist = true;
                }
                catch (AmazonS3Exception e)
                {
                    if (e.ErrorCode != "NotFound")
                    {
                        throw;
                    }

                    // only reach here when file doesn't exist
                }

                return exist;
            }
        }
    }
}