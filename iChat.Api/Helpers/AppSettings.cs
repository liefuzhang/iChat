namespace iChat.Api.Helpers
{
    public class AppSettings
    {
        public string JwtSecret { get; set; }
        public string GmailHost { get; set; }
        public string GmailUserName { get; set; }
        public string GmailPassword { get; set; }
        public string GmailPort { get; set; }
        public string GmailSsl { get; set; }
        public string FrontEndUrl { get; set; }
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string AwsFileBucketName { get; set; }
    }
}
