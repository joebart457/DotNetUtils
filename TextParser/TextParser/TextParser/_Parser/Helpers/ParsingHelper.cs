﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextParser._Tokenizer.Models;

namespace TextParser._Parser.Helpers
{
    public class ParsingHelper
    {
        private int _index = 0;
        private bool _bAtEnd = false;
        private Token? _current = null;
        private IEnumerable<Token> _tokens = new List<Token>();

        public bool init(IEnumerable<Token> tokens)
        {
            reset();
            _tokens = tokens;
            if (tokens.Count() == 0)
            {
                _bAtEnd = true;
                return false;
            }
            _current = tokens.First();
            return true;
        }
        public void reset()
        {
            _index = 0;
            _bAtEnd = false;
            _current = null;
        }

        public void advance()
        {
            if (_bAtEnd)
            {
                throw new IndexOutOfRangeException("Cannot advance past end");
            }
            _index++;
            if (_index >= _tokens.Count())
            {
                _bAtEnd = true;
                return;
            }
            _current = _tokens.ElementAt(_index);
        }

        public bool match(string type)
        {
            if (_bAtEnd || _current == null)
            {
                return false;
            }
            if (_current.Type == type)
            {
                advance();
                return true;
            }
            return false;
        }
        public bool match(Token token, string type)
        {
            if (token == null) return false;
            if (token.Type == type)
            {
                return true;
            }
            return false;
        }

        public bool lexMatch(string lexeme)
        {
            if (_bAtEnd || _current == null)
            {
                return false;
            }
            if (_current.Lexeme == lexeme)
            {
                advance();
                return true;
            }
            return false;
        }

        public Token previous()
        {
            if (_index - 1 >= _tokens.Count())
            {
                throw new IndexOutOfRangeException("failed getting previous token");
            }
            return _tokens.ElementAt(_index - 1);
        }

        public Token consume(string type, string errorMessage = "")
        {
            if (_bAtEnd)
            {
                throw new IndexOutOfRangeException($"at end when calling consume; {errorMessage}");
            }
            if (match(type))
            {
                return previous();
            }
            throw new ArgumentException($"{errorMessage} at {(!_bAtEnd ? current().ToString() : "null")}");
        }

        public bool peekMatch(int offset, string type)
        {
            if (_index + offset < _tokens.Count())
            {
                return _tokens.ElementAt(_index + offset).Type == type;
            }
            return false;
        }

        public bool atEnd()
        {
            return _bAtEnd;
        }

        public Token? current()
        {
            return _current;
        }
    }
}
