using ParsingCoreV2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace ParsingCoreV2.Implementation
{
    internal class ParserInterface : IParserInterface
    {
        private int _index = 0;
        private bool _atEnd = false;
        private IToken? _current = null;
        private IList<IToken> _tokens = new List<IToken>();
        public ParserInterface()
        {
        }

        public void Initialize(IList<IToken> tokens)
        {
            _tokens = tokens;
            _index = 0;
            _atEnd = tokens.Count == 0;
            _current = tokens.FirstOrDefault();
        }

        public void Initialize(IList<IToken> tokens, int index)
        {
            _tokens = tokens;
            _index = index;
            _atEnd = tokens.Count >= _index;
            _current = tokens.ElementAtOrDefault(index);
        }

        public void Advance()
        {
            if (_atEnd) throw new IndexOutOfRangeException("unable to advance past end");
            _index++;
            if (_index >= _tokens.Count) _atEnd = true;
            _current = _tokens.ElementAtOrDefault(_index);
        }

        public bool AdvanceIfMatch(string tokenType)
        {
            if (_atEnd || _current == null) return false;
            if (_current.Type != tokenType) return false;
            Advance();
            return true;
        }

        public bool Match(string tokenType)
        {
            if (_atEnd || _current == null) return false;
            return _current.Type == tokenType;
        }

        public bool Match(IToken token, string type)
        {
            return token.Type == type;
        }

        public bool MatchLexeme(string lexeme)
        {
            if (_atEnd || _current == null) return false;
            return _current.Lexeme == lexeme;
        }

        public bool PeekMatch(int offset, string type)
        {
            if (_index + offset < _tokens.Count())
            {
                return _tokens[_index + offset].Type == type;
            }
            return false;
        }

        public IToken Consume(string type, string errorMessage)
        {
            if (!Match(type) || _current == null) throw new Exception(errorMessage);
            return _current;
        }

        public IToken Consume(string type, Exception ex)
        {
            if (!Match(type) || _current == null) throw ex;
            return _current;
        }

        public bool AtEnd()
        {
            return _atEnd;
        }

        public IToken Current()
        {
            return _current ?? throw new IndexOutOfRangeException($"unable to retrieve current token at index {_index}");
        }

        public IToken Previous()
        {
            if (_index - 1 >= _tokens.Count())
            {
                throw new IndexOutOfRangeException("failed getting previous token");
            }
            return _tokens[_index - 1];
        }

        public IList<IToken> GetTokens()
        {
            return _tokens;
        }

        public int Index()
        {
            return _index;
        }
    }
}
