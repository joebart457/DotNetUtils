using ExpressionEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEvaluator
{
    internal class Parser
    {
        private List<CustomOperator> _operators = new List<CustomOperator>();

        public void RegisterCustomOperator(CustomOperator op)
        {
            _operators.Add(op);

            // TODO: make sorting more efficient 
            _operators = _operators.OrderBy(a => a.Priority).ToList();
        }


        public object Parse()
    }
}
