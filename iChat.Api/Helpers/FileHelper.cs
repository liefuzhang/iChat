using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using iChat.Api.Constants;
using iChat.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace iChat.Api.Helpers {
    public class FileHelper : IFileHelper {
        private readonly iChatContext _context;
        private readonly AppSettings _appSettings;

        public FileHelper(iChatContext context, IOptions<AppSettings> appSettings) {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file, int workspaceId) {
            var uploadToSubFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var client = new AmazonS3Client(_appSettings.AwsAccessKeyId, _appSettings.AwsSecretAccessKey, RegionEndpoint.APSoutheast2)) {
                using (var memoryStream = new MemoryStream()) {
                    file.CopyTo(memoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest {
                        InputStream = memoryStream,
                        Key = fileName,
                        BucketName = $"{_appSettings.AwsFileBucketName}/{uploadToSubFolder}",
                        CannedACL = S3CannedACL.PublicRead // TODO change to private
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }

            return fileName;
        }

        public async Task<FileStream> DownloadFileAsync(string savedName, int workspaceId) {
            var downloadFromSubFolder = iChatConstants.AwsBucketWorkspaceFileFolderPrefix + workspaceId;
            var downloadPath = Path.GetTempFileName();
            using (var client = new AmazonS3Client(_appSettings.AwsAccessKeyId, _appSettings.AwsSecretAccessKey, RegionEndpoint.APSoutheast2)) {
                using (var memoryStream = new MemoryStream()) {
                    var downloadRequest = new TransferUtilityDownloadRequest {
                        Key = savedName,
                        BucketName = $"{_appSettings.AwsFileBucketName}/{downloadFromSubFolder}",
                        FilePath = downloadPath
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.DownloadAsync(downloadRequest);
                }
            }

            return new FileStream(@"path\to\file", FileMode.Open);
        }
    }
}