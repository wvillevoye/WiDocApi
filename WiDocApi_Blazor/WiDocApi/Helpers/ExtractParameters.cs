using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_Blazor.WiDocApi.Services;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class ExtractParameters
    {

        public List<(string ParameterName, string? Type)> FromPath(string path)
        {
            var parameters = new List<(string, string?)>();
            var matches = System.Text.RegularExpressions.Regex.Matches(path, @"\{(\w+)(?::(\w+))?\}");

            foreach (Match match in matches)
            {
                string paramName = match.Groups[1].Value; // Capture the parameter name
                string paramType = match.Groups[2].Success ? match.Groups[2].Value :"string"; // Capture the type if available

                // If colon is found and paramType is not null, ensure paramName does not include the type
                if (!string.IsNullOrEmpty(paramType))
                {
                    paramName = paramName.Split(':')[0]; // Only keep the part before the colon
                }

                parameters.Add((paramName, paramType));
            }
            return parameters;
        }

        public void InitializeDynamicInputValues(ApiEndpoint endpoint)
        {
            var parameters = FromPath(endpoint.Path!);
            foreach (var (paramName, paramType) in parameters)
            {
                // Check if the dictionary already contains this parameter
                if (!endpoint.DynamicInputValues.ContainsKey(paramName))
                {
                    // Initialize with default value based on type without converting to string
                    endpoint.DynamicInputValues[paramName] = GetDefaultValueForType(paramType).ToString()!;
                }
            }
        }

        private object GetDefaultValueForType(string? paramType)
        {
            return paramType switch
            {
                "int" => 0,
                "bool" => false, // Keep as boolean
                "string" => string.Empty,
                "datetime" => DateTime.Now,
               // "enum" => GetEnumDefaultValue(),
                _ => string.Empty // Default to string for unknown types
            };
        }

        // Helper function to get the default enum value (typically the first one)
        //private object GetEnumDefaultValue()
        //{
        //    // If you want to pass a specific enum type, you can modify this function to handle that case.
        //    // For now, it returns a placeholder value.
        //    // Replace 'YourEnumType' with an actual enum type you want to handle
        //    Type enumType = typeof(YourEnumType); // Replace 'YourEnumType' with your actual enum type
        //    var values = Enum.GetValues(enumType);
        //    return values.Length > 0 ? values.GetValue(0) : 0; // Return the first value or 0 if no values found
        //}

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
