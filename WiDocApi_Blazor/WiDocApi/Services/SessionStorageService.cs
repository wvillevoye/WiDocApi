using Microsoft.JSInterop;
using System.Text.Json;

namespace WiDocApi_Blazor.WiDocApi.Services
{
    public class SessionStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        // Inject the IJSRuntime into the service class
        public SessionStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Method to get a value from sessionStorage
        public async Task<string> GetFromSessionStorage(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);
        }

        // Method to set a value in sessionStorage
        public async Task SetInSessionStorage(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, value);
        }
        public async Task<bool> ApiKeyExists()
        {
            var apiKey = await GetFromSessionStorage("apiKey");
            return !string.IsNullOrEmpty(apiKey);
        }

    }
    public class ApiStateService
    {
        public bool ApiKeyExists { get; set; }
    }
}
