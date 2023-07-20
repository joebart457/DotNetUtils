using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliParser.Extensions
{
    internal static class TypeExtensions
    {
        public static bool HasImplicitConversion(this Type toType, Type fromType)
        {
            return toType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == "op_Implicit" && mi.ReturnType == toType)
                .Any(mi => {
                    ParameterInfo? pi = mi.GetParameters().FirstOrDefault();
                    return pi != null && pi.ParameterType == fromType;
                });
        }

        public static MethodInfo? GetImplicitConversion(this Type toType, Type fromType)
        {
            return toType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == "op_Implicit" && mi.ReturnType == toType)
                .FirstOrDefault(mi => {
                    ParameterInfo? pi = mi.GetParameters().FirstOrDefault();
                    return pi != null && pi.ParameterType == fromType;
                });
        }

    }
}
