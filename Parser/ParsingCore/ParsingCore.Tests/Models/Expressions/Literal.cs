using ParsingCore.Interfaces;
using ParsingCore.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Models.Expressions
{
    internal class Literal: BaseExpression
    {
        public object? Value { get; set; }
        public Literal(object? value)
        {
            Value = value;
        }
        public override object? Accept(IVisitor<InterpreterTm, object?> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
