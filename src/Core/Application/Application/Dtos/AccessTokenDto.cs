namespace Core.Application.Dtos
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }

    }
}
