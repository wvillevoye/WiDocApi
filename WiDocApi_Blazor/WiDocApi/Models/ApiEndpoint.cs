using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WiDocApi_Blazor.WiDocApi.Models
{
    public class ApiEndpoint
    {
        public int Id { get; set; }

        public string BaseUrl { get; set; } = string.Empty;

        public string Group { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Use an enum to represent HTTP methods.
        public WiDocApiHttpMethod Method { get; set; } = WiDocApiHttpMethod.UNKNOWN;

        public bool RequiresInput { get; set; } = true;

        [Range(0, int.MaxValue, ErrorMessage = "Cache duration must be a non-negative value.")]
        public int CacheDurationMinutes { get; set; } = 0;

        public bool Active { get; set; } = true;

        public string Payload { get; set; } = string.Empty;

        public string? ApiResponse { get; set; }

        public bool IsLoading { get; set; }

        public bool HasError { get; set; }

        public string? ApiRequest { get; set; }

        public string? Curl { get; set; }

        private Dictionary<string, string> _dynamicInputValues = new();

        public Dictionary<string, string> DynamicInputValues
        {
            get => _dynamicInputValues;
            set
            {
                _dynamicInputValues = value;
                TrimDictionaryValues();
            }
        }

        // Method to trim all values in the DynamicInputValues dictionary
        public void TrimDictionaryValues()
        {
            foreach (var key in _dynamicInputValues.Keys.ToList())
            {
                if (_dynamicInputValues[key] != null)
                {
                    _dynamicInputValues[key] = _dynamicInputValues[key].Trim();
                }
            }
        }
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
