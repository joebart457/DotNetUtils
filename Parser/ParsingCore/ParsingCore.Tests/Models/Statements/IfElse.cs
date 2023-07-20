using ParsingCore.Interfaces;
using ParsingCore.Tests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Models.Statements
{
    internal class IfElse : BaseStatement
    {
        public override void Accept(IVisitor<InterpreterTm> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
