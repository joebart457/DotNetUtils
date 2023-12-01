using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace ParserLite.Exceptions
{
    public class ParsingException: System.Exception
    {
        public IToken Token { get; set; }

        public ParsingException(IToken token, string message)
            : base(message)
        {
            Token = token;
        }

        public string What()
        {
            return $"[{Token.Location}] {Message}";
        }
    }
}
