using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    internal static class MockEngine
    {
        private static Dictionary<Type, object?> Defaults = new Dictionary<Type, object?>();

        public static void RegisterMockForType<Ty>(Ty mock)
        {
            Defaults.Add(typeof(Ty), mock);
        }

        public static object? GetMockForType(Type type)
        {
            if (Defaults.TryGetValue(type, out object? value)) return value;
            throw new KeyNotFoundException(type.FullName);
        }

    }
}
