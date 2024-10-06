using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public static class WiDoApiUtils
    {




     

        private static Dictionary<string, Dictionary<string, T>> BuildSelectDictionary<T>(string name, Dictionary<string, T> listDb)
        {
            var result = new Dictionary<string, Dictionary<string, T>>();
            result.Add(name, listDb);
            return result;
        }
        public enum SelectValueType
        {
            Text,
            Number
        }

        public static Dictionary<string, Dictionary<string, T>> CreateSelectInput<T>(string name, SelectValueType inputType, Dictionary<string, T> listDb)
        {
            if (inputType == SelectValueType.Number && typeof(T) == typeof(int))
            {
                // Cast to Dictionary<string, int> if inputType is Number
                var intDict = listDb as Dictionary<string, int>;
                return BuildSelectDictionary(name, intDict as Dictionary<string, T>);
            }
            else if (inputType == SelectValueType.Text && typeof(T) == typeof(string))
            {
                // Cast to Dictionary<string, string> if inputType is Text
                var stringDict = listDb as Dictionary<string, string>;
                return BuildSelectDictionary(name, stringDict as Dictionary<string, T>);
            }

            throw new ArgumentException("Invalid input type or dictionary type mismatch.");
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


    public static class DictionaryExtensions
    {
        public static Dictionary<string, Dictionary<string, T>> AddWithChain<T>(
            this Dictionary<string, Dictionary<string, T>> dictionary,
            string key,
            WiDoApiUtils.SelectValueType inputType,
            Dictionary<string, T> value)
        {
            var newEntry = WiDoApiUtils.CreateSelectInput(key, inputType, value); // Use CreateSelectInput

            if (!dictionary.ContainsKey(key))
            {
                // Add the key and value if the key doesn't already exist
                dictionary.Add(key, newEntry[key]);
            }
            else
            {
                // If the key exists, merge the new dictionary with the existing one
                foreach (var item in newEntry[key])
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
