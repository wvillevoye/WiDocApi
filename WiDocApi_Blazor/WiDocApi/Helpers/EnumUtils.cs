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
        // Method to get the names of the enum as a list of strings
        public static List<string> EnumToList<T>() where T : Enum
        {
            return new List<string>(Enum.GetNames(typeof(T)));
        }

        public static Dictionary<string, List<string>> DBToList(string name , List<string> listDb)
        {
            var _res = new Dictionary<string, List<string>>();
            _res.Add(name, listDb!);
            return _res;
        }


        // Method to create a dictionary of enum lists
        public static Dictionary<string, List<string>> CreateEnumLists(params (string key, Type enumType)[] enums)
        {
            var enumLists = new Dictionary<string, List<string>>();

            foreach (var (key, enumType) in enums)
            {
                // Get the method info for EnumToList<T>
                var methodInfo = typeof(EnumUtils).GetMethod(nameof(EnumToList), new Type[0]);

                // Make a generic method for the enum type
                var genericMethod = methodInfo!.MakeGenericMethod(enumType);

                // Invoke the generic method
                var enumList = (List<string>)genericMethod.Invoke(null, null)!;
                enumLists[key] = enumList;
            }

            return enumLists;
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
    

}
