using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiDocApi_Blazor.WiDocApi.Models;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class MappingEndpoints
    {
        public List<ApiEndpoint> MapEndpointInfoListToApiEndpointList(List<EndpointInfo> endpointInfos)
        {
            var apiEndpoints = new List<ApiEndpoint>();

            foreach (var endpointInfo in endpointInfos)
            {
                // Map each EndpointInfo to an ApiEndpoint and add to the list
                apiEndpoints.Add(MapEndpointInfoToApiEndpoint(endpointInfo));
            }

            return apiEndpoints;
        }
        private ApiEndpoint MapEndpointInfoToApiEndpoint(EndpointInfo endpointInfo)
        {
            return new ApiEndpoint
            {
                Id = endpointInfo.Id,
                BaseUrl = endpointInfo.BaseUrl,
                Group = endpointInfo.Group,
                Path = endpointInfo.Path,
                Description = endpointInfo.Description,
                Method = endpointInfo.Method, // Convert enum to string
                RequiresInput = endpointInfo.RequiresInput,
                CacheDurationMinutes = endpointInfo.CacheDurationMinutes,

                // Additional properties in ApiEndpoint that are not in EndpointInfo
                Payload = string.Empty,
                ApiResponse = string.Empty,
                IsLoading = false,
                HasError = false,
                ApiRequest = string.Empty,
                Curl = string.Empty,
                Active = true,
                DynamicInputValues = new Dictionary<string, string>()
            };
        }
    }
}
