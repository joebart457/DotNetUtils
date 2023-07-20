using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEvaluator.Models
{
    internal class CustomOperator
    {
        public MethodInfo? MethodInfo { get; set; }
        public string Name { get; set; } = "";
        public int Priority { get; set; }
    }
}
