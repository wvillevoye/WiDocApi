namespace WiDocApi_Blazor.WiDocApi.Models
{
    public class CachedApiResponse
    {
        public string Response { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
