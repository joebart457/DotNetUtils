using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework
{
    internal class TestEngine
    {
        
        public static void RunTestsForAssembly(string assemblyLocation)
        {
            var asm = Assembly.LoadFrom(assemblyLocation);

            var types = asm.GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    RunTests(method);
                }
            }

        }
        private static void RunTests(MethodInfo? methodInfo)
        {
            if (methodInfo == null) return;
            var parameters = methodInfo.GetParameters();
            var args = new List<object?>();
            foreach (var parameter in parameters)
            {
                args.Add(MockEngine.GetMockForType(parameter.ParameterType));
            }

            

        }
    }
}
