using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_Blazor.WiDocApi.Services;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class ExtractParameters
    {

        public List<string> FromPath(string path)
        {
            var parameters = new List<string>();
            var matches = System.Text.RegularExpressions.Regex.Matches(path, @"\{(.*?)\}");

            foreach (var match in matches)
            {
                parameters.Add(match.ToString()!.Trim('{', '}'));
            }

            return parameters;
        }

        public void InitializeDynamicInputValues(ApiEndpoint endpoint)
        {

            var parameters = FromPath(endpoint.Path!);
            foreach (var param in parameters)
            {
                // Voeg elke parameter toe aan de dictionary met een lege string als waarde
                if (!endpoint.DynamicInputValues.ContainsKey(param))
                {
                    endpoint.DynamicInputValues[param] = string.Empty;
                }
            }
        }

        public  string CreateCurl(ApiEndpoint endpoint, bool IsValidApiKeyConfigured , string apiKey)
        {

            var sb = new StringBuilder();
            sb.Append($"curl -X '{endpoint.HttpMethod.ToString()}' \n");
            sb.Append($"'{endpoint.ApiRequest}' \n");
            sb.Append($"-H 'Content-Type: application/json' \n");
            if (IsValidApiKeyConfigured)
            {
                sb.Append($"-H 'X-Api-Key: {apiKey}' \n");
            }
            if (endpoint.HttpMethod.ToString() == "POST" || endpoint.HttpMethod.ToString() == "PUT" || endpoint.HttpMethod.ToString() == "PATCH")
            {
                sb.Append($"-d '{endpoint.Payload}'");
            }
            return sb.ToString();
        }

    }
}
