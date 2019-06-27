namespace iChat.Api.Dtos
{
    public class ResetPasswordDto {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}