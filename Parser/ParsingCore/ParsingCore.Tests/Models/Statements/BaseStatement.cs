using ParsingCore.Interfaces;
using ParsingCore.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Models.Statements
{
    internal class BaseStatement : IAcceptor<InterpreterTm>
    {
        public virtual void Accept(IVisitor<InterpreterTm> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
