using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WiDocApi_Blazor.WiDocApi.Models
{
    public class ApiEndpoint
    {

        private WiDocApiHttpMethod _httpMethod;

        [JsonIgnore]
        public WiDocApiHttpMethod HttpMethod
        {
            get => _httpMethod;
            set => _httpMethod = value;
        }


        [JsonPropertyName("httpMethod")]
        public string HttpMethodString
        {
            get => _httpMethod.ToString();
            set
            {
                if (Enum.TryParse<WiDocApiHttpMethod>(value, true, out var parsedEnum))
                {
                    _httpMethod = parsedEnum;
                }
                else
                {
                    _httpMethod = WiDocApiHttpMethod.UNKNOWN;
                }
            }
        }


        public int Id { get; set; }

        public string BaseUrl { get; set; } = string.Empty;

        public string Group { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

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


        public Dictionary<string, Dictionary<string, string>> EnumLists { get; set; } = new();


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
