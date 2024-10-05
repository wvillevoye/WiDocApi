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


        //public static Dictionary<string, List<string>> DBToList(string name, List<string> listDb)
        //{
        //    var result = new Dictionary<string, List<string>>();
        //    result.Add(name, listDb!);
        //    return result;
        //}

        public static Dictionary<string, Dictionary<string, string>> DBToList(string name, Dictionary<string, string> listDb)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            result.Add(name, listDb);
            return result;
        }


        public static Dictionary<string, Dictionary<string, string>> EnumToDictionary<T>() where T : Enum
        {
            var enumTypeName = typeof(T).Name;

            // Convert enum names to a dictionary where the key and value are the same
            var enumValues = Enum.GetNames(typeof(T)).ToDictionary(name => name, name => name);

            // Create the dictionary with the enum type name as the key and the dictionary of enum values
            var result = new Dictionary<string, Dictionary<string, string>>
            {           
                { enumTypeName, enumValues }
            };

                return result;
        }
      
        
        public static Dictionary<string, string> ListToDictionary(List<string> list)
        {
            return list.ToDictionary(item => item, item => item);
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
        public static Dictionary<string, Dictionary<string, string>> AddWithChain(
            this Dictionary<string, Dictionary<string, string>> dictionary, string key, Dictionary<string, string> value)
        {
            if (!dictionary.ContainsKey(key))
            {
                // Add the key and value if the key doesn't already exist
                dictionary.Add(key, value);
            }
            else
            {
                // If the key exists, merge the new dictionary with the existing one
                foreach (var item in value)
                {
                    dictionary[key][item.Key] = item.Value; // This updates existing keys or adds new ones
                }
            }

            return dictionary; // Return the dictionary to enable chaining
        }
    }

    public class SelectConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // Check if the route key exists in the values
            if (values.TryGetValue(routeKey, out var value) && value is string stringValue)
            {
                // Allow any string value as valid
                return true; // Allows any string

                // If you want to add some specific conditions, you can still do that.
                // Example:
                // return stringValue == "select" || stringValue == "option1" || stringValue == "option2" || true;
            }

            // If the key doesn't exist or isn't a string, return false
            return false;
        }
    }

}
