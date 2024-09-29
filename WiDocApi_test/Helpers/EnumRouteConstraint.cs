using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiDocApi_test.Helpers
{
    public class EnumRouteConstraint<T> : IRouteConstraint where T : struct, Enum
    {
        public bool Match(HttpContext httpContext,
                          IRouter route,
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
