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
}
