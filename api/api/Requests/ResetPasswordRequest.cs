namespace api.Requests
{
    public class ResetPasswordRequest
    {
        public string? PasswordResetToken { get; set; }
        public string? Password { get; set; }
    }
}
