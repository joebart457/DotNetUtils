using ParsingCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Models
{
    internal class TokenTm : IToken
    {
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public ILocation Location { get; set; }

        public TokenTm(string type, string lexeme, ILocation location)
        {
            Type = type;
            Lexeme = lexeme;
            Location = location;
        }
    }
}
