﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiDocApi_Blazor;
using WiDocApi_Blazor.WiDocApi.Helpers;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_Blazor.WiDocApi.Services;

namespace WiDocApi_Blazor.Configuration.WiDocApi
{
    public class WiDocApiDependencyInjection : IDependency
    {
        public void AddDependencies(IServiceCollection services, WiDocApiApikeySettings apikeySettings)
        {

            if (!services.Any(service => service.ServiceType == typeof(HttpClient)))
            {
                // Voeg HttpClient toe aan de service collection als het nog niet is toegevoegd
                services.AddScoped(sp => new HttpClient());
            }
            services.AddScoped<WiDocApiScript>();
            services.AddScoped<WiDocApiApiKeyAuthFilter>();
            services.AddScoped<StartEndpoint>();
            services.AddScoped<LoadEndpoints>();
            services.AddScoped<HttpMethodClassMapper>();
            services.AddScoped<SessionStorageService>();
            services.AddScoped<ExtractParameters>();
            services.AddSingleton(sp => apikeySettings);

            services.AddSingleton<ApiStateService>();

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("select", typeof(SelectConstraint));
            });

        }

    }
}
