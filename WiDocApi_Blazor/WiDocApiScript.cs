using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiDocApi_Blazor
{
        public class WiDocApiScript : IAsyncDisposable
        {
            private readonly Lazy<Task<IJSObjectReference>> moduleTask;

            public WiDocApiScript(IJSRuntime jsRuntime)
            {
                moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/WiDocApi_Blazor/WiDocApiScript.js").AsTask());
            }

        public async ValueTask CopyToClipboard(string textToCopy)
        {
            try
            {
                var module = await moduleTask.Value;
                await module.InvokeVoidAsync("copyToClipboard", textToCopy);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async ValueTask DownloadJsonFile(string jsonObject)
        {
            try
            {
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string filename = $"data_{timestamp}.json";
                    var module = await moduleTask.Value;
                    await module.InvokeVoidAsync("downloadJsonFile", filename, jsonObject);

            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async ValueTask FormatJson()
        {
            try
            {
                var module = await moduleTask.Value;
                await module.InvokeVoidAsync("formatJson");
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        public async ValueTask DisposeAsync()
            {
                if (moduleTask.IsValueCreated)
                {
                    var module = await moduleTask.Value;
                    await module.DisposeAsync();
                }
            }
        }
    }



