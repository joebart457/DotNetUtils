
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

        public override string ToString()
        {
            return $"[Ln. {Token.Start.Line}, Col. {Token.Start.Column}] {Message}";
        }
    }
}
