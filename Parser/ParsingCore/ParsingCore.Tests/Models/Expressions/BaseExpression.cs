using ParsingCore.Interfaces;
using ParsingCore.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Models.Expressions
{
    internal class BaseExpression : IAcceptor<InterpreterTm, object?>
    {
        public virtual object? Accept(IVisitor<InterpreterTm, object?> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
