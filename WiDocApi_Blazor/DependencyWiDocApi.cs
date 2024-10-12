using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WiDocApi_Blazor.WiDocApi.Models;

namespace WiDocApi_Blazor
{
    public static class DependencyWiDocApi
    {
        public static void AddSiteWiDocApi(this IServiceCollection services, WiDocApiApikeySettings? apikeySettings = default)
        {

            apikeySettings ??= new WiDocApiApikeySettings
            {
                ApiKey = string.Empty, 
                ApiKeyHeaderName = "X-Api-Key" 
            };



            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => typeof(IDependency).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });

            foreach (var type in types)
            {
                var dependency = Activator.CreateInstance(type) as IDependency;
                dependency?.AddDependencies(services, apikeySettings);
            }
        }
    }

    public interface IDependency
    {
        void AddDependencies(IServiceCollection services, WiDocApiApikeySettings apikeySettings);
    }

}
