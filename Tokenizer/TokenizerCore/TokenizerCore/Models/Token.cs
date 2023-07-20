using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace TokenizerCore.Model
{
    public class Token: IToken
    {
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public ILocation Location { get; set; }

        public Token(TokenizerRule rule, int line, int column)
        {
            Lexeme = rule.ReplaceWith;
            Type = rule.Type;
            Location = new Location(line, column);
        }

        public Token(string type, string lexeme, int nRow, int nColumn)
        {
            Lexeme = lexeme;
            Type = type;
            Location = new Location(nRow, nColumn);
        }

        public override string ToString()
        {
            return $"Token({Type}|{Lexeme}) at {Location}";
        }
    }
}
