using System;
using System.Collections;
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
        public ILocation Start { get; set; }
        public ILocation End { get; set; }

        public Token(TokenizerRule rule, ILocation start, ILocation end)
        {
            Lexeme = rule.ReplaceWith;
            Type = rule.Type;
            Start = start;
            End = end;
        }

        public Token(string type, string lexeme, ILocation start, ILocation end)
        {
            Lexeme = lexeme;
            Type = type;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"Token({Type}, {Lexeme}) at {Start}";
        }

    }
}
