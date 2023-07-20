using ExpressionEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEvaluator
{
    internal class EvaluatorEngine
    {
        private Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

        private List<CustomOperator> _operators = new List<CustomOperator>();
        public void RegisterCustomAction<Ty1,Ty2,TyRet>(string name, Func<Ty1,Ty2,TyRet> func, int priority = 99) 
        {
            _methods.Add(func.Method.Name, func.Method);
        }
    }
}
