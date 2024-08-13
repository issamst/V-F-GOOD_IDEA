namespace Asp_Net_Good_idea.Dto.UserDto
{
    public record ResetPasswordDto
    {
        public string TEID { get; set; }
        public string Email { get; set; }
        public string EmailToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }



    }
}
