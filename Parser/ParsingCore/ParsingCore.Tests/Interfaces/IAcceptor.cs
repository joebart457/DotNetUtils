using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Interfaces
{
    public interface IAcceptor<Ty>
    {
        public void Accept(IVisitor<Ty> visitor);
    }

    public interface IAcceptor<Ty, TyResult>
    {
        public TyResult Accept(IVisitor<Ty, TyResult> visitor);
    }
}
