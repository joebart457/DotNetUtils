using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCoreV2.Interfaces
{
    public interface IParsable<Ty>
    {
        public bool CanParse(IParserInterface parser);
        public Ty Parse(IParserInterface parser);
    }
}
