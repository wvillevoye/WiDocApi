
namespace WiDocApi_Blazor.WiDocApi.Models
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EndpointInfo
    {
        public int Id { get; set; }


        public string BaseUrl { get; set; } = string.Empty;

        public string Group { get; set; } = "Group";

        // Consider adding a more descriptive name if the 'Path' is a relative endpoint path.
        public string Path { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Use an enum instead of a string to represent HTTP methods.
        public WiDocApiHttpMethod Method { get; set; }

        public bool RequiresInput { get; set; } = true;

        [Range(0, int.MaxValue, ErrorMessage = "Cache duration must be a non-negative value.")]
        public int CacheDurationMinutes { get; set; } = 0;
        public bool Active { get; set; } = true;


    }

    // Enum to represent HTTP methods.
    public enum WiDocApiHttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH,
        HEAD,
        OPTIONS,
        UNKNOWN
    }

}