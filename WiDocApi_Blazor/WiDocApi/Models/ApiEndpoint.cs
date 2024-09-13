namespace WiDocApi_Blazor.WiDocApi.Models
{
    public class ApiEndpoint
    {

        public int Id { get; set; }
        public string? BaseUrl { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
        public string? Method { get; set; }
        public bool RequiresInput { get; set; }
        public string Payload { get; set; } = string.Empty;
        public string? ApiResponse { get; set; }
        public bool IsLoading { get; set; }
        public bool HasError { get; set; }
        public string? ApiRequest { get; set; }
        public string? Group { get; set; }
        public string? Curl { get; set; }
        public int CacheDurationMinutes { get; set; } 
        public bool Active { get; set; } = true;
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
}
