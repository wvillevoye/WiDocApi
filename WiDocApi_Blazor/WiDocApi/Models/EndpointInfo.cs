
namespace WiDocApi_Blazor.WiDocApi.Models
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EndpointInfo
    {
        public int Id { get; set; }
        public string Group { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool RequiresInput { get; set; } = true;
        [Range(0, int.MaxValue, ErrorMessage = "Cache duration must be a non-negative value.")]
        public int CacheDurationMinutes { get; set; } = 0;

        public Dictionary<string, List<string>> EnumLists { get; set; } = new();

      

        public bool Active { get; set; } = true;

    }


}