using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace ParsingCore.Interfaces
{
    public interface IParserInterface
    {
        public void Initialize(IList<IToken> tokens);
        public void Initialize(IList<IToken> tokens, int index);
        public bool Match(string tokenType);
        public bool Match(IToken token, string type);
        public void Advance();
        public bool AdvanceIfMatch(string tokenType);
        public bool MatchLexeme(string lexeme);
        public IToken Previous();
        public IToken Consume(string type, string errorMessage);
        public IToken Consume(string type, Exception exception);
        public bool PeekMatch(int offset, string type);
        public bool AtEnd();
        public IToken Current();
        public IList<IToken> GetTokens();
        public int Index();
    }
}
