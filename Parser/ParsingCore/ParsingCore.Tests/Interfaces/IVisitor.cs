using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Interfaces
{
    public interface IVisitor<Ty>
    {
        public Ty Value { get; set; }
        public void Visit(IAcceptor<Ty> acceptor);
    }

    public interface IVisitor<Ty, TyResult>
    {
        public Ty Value { get; set; }
        public TyResult Visit(IAcceptor<Ty, TyResult> acceptor);
    }
}
