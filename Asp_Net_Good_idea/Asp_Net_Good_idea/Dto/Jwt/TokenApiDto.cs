namespace Asp_Net_Good_idea.Dto.Jwt
{
    public class TokenApiDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Messager { get; set; } = string.Empty;

    }
}
