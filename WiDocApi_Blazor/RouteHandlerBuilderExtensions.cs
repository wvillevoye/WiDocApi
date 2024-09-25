﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using WiDocApi_Blazor.WiDocApi.Models;

public static class RouteHandlerBuilderExtensions
{
      public static class WiDocApiStorage
    {
        // Correct initialization of the list
        public static List<ApiEndpoint> WiDocApiList = new List<ApiEndpoint>();

        private static int _endpointIdCounter = 1; // Start the counter from 1

        public static int GetNextEndpointId()
        {
            return _endpointIdCounter++;
        }
    }

   

    // Your AddWiDocApiEndpoints extension method
    public static RouteHandlerBuilder AddWiDocApiEndpoints(this RouteHandlerBuilder builder, EndpointInfo endpointInfo)
    {
       
        builder.Add(endpointBuilder =>
        {
            // Create a new instance for each endpoint to avoid reference duplication
            var currentEndpointInfo = new ApiEndpoint
            {
                Id = endpointInfo.Id == 0? WiDocApiStorage.GetNextEndpointId(): endpointInfo.Id,
                Group = endpointInfo.Group,
                Description = endpointInfo.Description,
                RequiresInput = endpointInfo.RequiresInput,
                CacheDurationMinutes = endpointInfo.CacheDurationMinutes,
                Active = endpointInfo.Active
            };

            var httpMethodMetadata = endpointBuilder.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();
            if (httpMethodMetadata != null)
            {
                currentEndpointInfo.HttpMethod = ParseHttpMethod(httpMethodMetadata.HttpMethods.First());
            }

            // Capture the route pattern
            if (endpointBuilder is RouteEndpointBuilder routeEndpointBuilder)
            {
                currentEndpointInfo.Path = $"{routeEndpointBuilder.RoutePattern.RawText}";

               
            }
            else
            {
                // Fallback in case it's not a RouteEndpointBuilder
                currentEndpointInfo.Path = $"[Unknown Route]";
            }
            if (!WiDocApiStorage.WiDocApiList.Any(e => e.HttpMethod == currentEndpointInfo.HttpMethod && e.Path == currentEndpointInfo.Path))
            {
                if (currentEndpointInfo.Active)
                {
                    WiDocApiStorage.WiDocApiList.Add(currentEndpointInfo);
                }
            }
        });

        return builder;
    }



    // Method to parse HTTP methods
    private static WiDocApiHttpMethod ParseHttpMethod(string httpMethod)
    {
        return httpMethod.ToUpper() switch
        {
            "GET" => WiDocApiHttpMethod.GET,
            "POST" => WiDocApiHttpMethod.POST,
            "PUT" => WiDocApiHttpMethod.PUT,
            "DELETE" => WiDocApiHttpMethod.DELETE,
            "PATCH" => WiDocApiHttpMethod.PATCH,
            "HEAD" => WiDocApiHttpMethod.HEAD,
            "OPTIONS" => WiDocApiHttpMethod.OPTIONS,
            _ => WiDocApiHttpMethod.UNKNOWN
        };
    }
}