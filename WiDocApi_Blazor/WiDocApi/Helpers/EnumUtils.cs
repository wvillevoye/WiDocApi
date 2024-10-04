using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public static class EnumUtils
    {
      

        public static Dictionary<string, List<string>> DBToList(string name, List<string> listDb)
        {
            var result = new Dictionary<string, List<string>>();
            result.Add(name, listDb!);
            return result;
        }




        public static Dictionary<string, List<string>> EnumToDictionary<T>() where T : Enum
        {
            var enumTypeName = typeof(T).Name;
            var enumValues = Enum.GetNames(typeof(T)).ToList();

            // Create the dictionary with the enum name as the key and the list of enum values as the value
            var result = new Dictionary<string, List<string>>
            {
                { enumTypeName, enumValues }
            };

            return result;
        }

    }

    public class EnumRouteConstraint<T> : IRouteConstraint where T : struct, Enum
    {
        public bool Match(HttpContext? httpContext,
                          IRouter? route,
                          string routeKey,
                          RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value) && value is string stringValue)
            {
                // Try to parse the enum from the string
                return Enum.TryParse<T>(stringValue, true, out _);
            }
            return false;
        }
    }
    public static class DictionaryExtensions
    {
        public static Dictionary<string, List<string>> AddWithChain(
            this Dictionary<string, List<string>> dictionary, string key, List<string> value)
        {
            dictionary.Add(key, value);
            return dictionary; // Return the dictionary to enable chaining
        }
    }

}
