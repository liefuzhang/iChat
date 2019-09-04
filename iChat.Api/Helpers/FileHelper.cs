using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using iChat.Api.Constants;
using iChat.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly iChatContext _context;
        private readonly AppSettings _appSettings;

        public FileHelper(iChatContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file, int workspaceId)
        {
            if (file.Length > 10 * 1024 * 1024) {
                throw new Exception("Max file size 10MB.");
            }

            var uploadToSubFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var client = new AmazonS3Client(_appSettings.AwsAccessKeyId, _appSettings.AwsSecretAccessKey, RegionEndpoint.APSoutheast2)) {
                const int maxFileCount = 500;
                var listObjectsRequest = new ListObjectsRequest();
                listObjectsRequest.BucketName = _appSettings.AwsFileBucketName;
                var listObjectsResponse = await client.ListObjectsAsync(listObjectsRequest);
                if (listObjectsResponse.S3Objects.Count > maxFileCount) {
                    throw new Exception("Max file count reached.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = $"{_appSettings.AwsFileBucketName}/{uploadToSubFolder}",
                        CannedACL = S3CannedACL.Private
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }

            return fileName;
        }

        public async Task<Stream> DownloadFileAsync(string savedName, int workspaceId)
        {
            var downloadFromSubFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;
            var downloadPath = Path.GetTempFileName();
            using (var client = new AmazonS3Client(_appSettings.AwsAccessKeyId, _appSettings.AwsSecretAccessKey, RegionEndpoint.APSoutheast2))
            {
                var downloadRequest = new TransferUtilityDownloadRequest
                {
                    Key = savedName,
                    BucketName = $"{_appSettings.AwsFileBucketName}/{downloadFromSubFolder}",
                    FilePath = downloadPath
                };

                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.DownloadAsync(downloadRequest);
            }

            var bufferSize = 4096;
            var stream = new FileStream(downloadPath, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize,
                FileOptions.DeleteOnClose);

            return stream;
        }

        public async Task DeleteFileAsync(string savedName, int workspaceId)
        {
            var deleteFromSubFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;

            using (var client = new AmazonS3Client(_appSettings.AwsAccessKeyId, _appSettings.AwsSecretAccessKey, RegionEndpoint.APSoutheast2))
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    Key = savedName,
                    BucketName = $"{_appSettings.AwsFileBucketName}/{deleteFromSubFolder}"
                };

                await client.DeleteObjectAsync(deleteObjectRequest);
            }
        }
    }
}