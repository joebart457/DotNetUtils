using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;
using TokenizerCore.Model;
using TokenizerCore.Models.Constants;

namespace TokenizerCore
{
	public class TokenizerSettings
	{
		/// <summary>
		/// Specifies which symbols are allowed inside of words
		/// </summary>
		public string WordIncluded { get; set; } = "";
		public string CatchAllType { get; set; } = "";
		public bool SkipWhiteSpace { get; set; } = true;
		public bool CommentsAsTokens { get; set; } = false;
		public bool IgnoreCase { get; set; } = false;

		/// <summary>
		/// When <value>true</value>, counts tabs and newlines as one char column and does not add to the row counter
		/// </summary>
		public bool AllOneLine { get; set; }
		public bool AllowNegatives { get; set; } = false;
		public char NegativeChar { get; set; } = '-';
		public bool NewlinesAsTokens { get; set; } = false;
		public static TokenizerSettings Default { get { return new TokenizerSettings(); } }
	}


	public class Tokenizer
	{
		private bool _bAtEnd;
		private int _nIndex;
		private int _nRow;
		private int _nColumn;
		private char _cCurrent;
		private string _text = "";
		private IEnumerable<TokenizerRule> _rules = new List<TokenizerRule>();
		TokenizerSettings _settings;
		public Tokenizer(IEnumerable<TokenizerRule> rules, TokenizerSettings? settings = null)
		{
			_rules = rules.OrderBy((rule) => rule.Length).Reverse();
			_settings = settings ?? TokenizerSettings.Default;
		}

		public IEnumerable<IToken> Tokenize(string text, bool bDebug = false)
		{
			Initialize(text);
			while (!_bAtEnd)
			{
				var token = Next();
				if (bDebug)
				{
					Console.Write($"{token}, ");
				}
				if (!_settings.CommentsAsTokens &&
					(token.Type == BuiltinTokenTypes.EndOfLineComment || token.Type == BuiltinTokenTypes.MultiLineComment)) continue;
				yield return token;
			}
			Reset();
		}

		private bool Initialize(string text)
		{
			Reset();
			_text = text;
			if (_text.Length == 0)
			{
				_bAtEnd = true;
				return false;
			}
			_cCurrent = _text[0];
			return true;
		}


		private void Reset()
		{
			_text = "";
			_nIndex = 0;
			_nRow = 0;
			_nColumn = 0;
			_bAtEnd = false;
		}

		private Token Next()
		{
			while (!_bAtEnd)
			{

				if (_cCurrent == '\0')
				{
					Advance();
				}

				if (char.IsWhiteSpace(_cCurrent))
				{
					if (_settings.SkipWhiteSpace)
					{
						if (!_settings.NewlinesAsTokens)
						{
                            Advance();
                            continue;
                        }
						if (LookAhead(1) == "\r\n")
						{
							Advance();
							Advance();
							return new Token(BuiltinTokenTypes.Newline, "\r\n", _nRow, _nColumn); 
						}

					}
					else
					{
						if (_cCurrent == ' ')
						{
							Advance();
							return new Token(BuiltinTokenTypes.Space, _cCurrent.ToString(), _nRow, _nColumn);
						}
						if (_cCurrent == '\t')
						{
							Advance();
							return new Token(BuiltinTokenTypes.Tab, _cCurrent.ToString(), _nRow, _nColumn);
						}
						if (_cCurrent == '\r')
						{
							Advance();
							return new Token(BuiltinTokenTypes.CarriageReturn, _cCurrent.ToString(), _nRow, _nColumn);
						}
						if (_cCurrent == '\n')
						{
							Advance();
							return new Token(BuiltinTokenTypes.LineFeed, _cCurrent.ToString(), _nRow, _nColumn);
						}
					}

				}

                if (_cCurrent == '_' || char.IsLetter(_cCurrent))
                {
                    return Word();
                }


                foreach (TokenizerRule rule in _rules)
				{
					if (CompareRule(LookAhead(rule.Length), rule))
					{
						Advance(rule.Length);

						if (rule.Type == BuiltinTokenTypes.EndOfLineComment)
						{
							return EndOfLineComment();
						}

						if (rule.Type == BuiltinTokenTypes.MultiLineComment)
						{
							return GetEnclosedToken(rule);
						}

						if (rule.Type == BuiltinTokenTypes.String && rule.IsEnclosed)
						{
							return GetStringToken(rule.EnclosingRight);
						}

						if (rule.IsEnclosed)
						{
							return GetEnclosedToken(rule);
						}

						return new Token(rule, _nRow, _nColumn);
					}
				}

				if (_cCurrent == '_' || char.IsLetter(_cCurrent))
				{
					return Word();
				}

				if (char.IsDigit(_cCurrent) || (_settings.AllowNegatives && _cCurrent == _settings.NegativeChar))
				{
					return Number();
				}



				Token result = new Token(string.IsNullOrWhiteSpace(_settings.CatchAllType) ? _cCurrent.ToString() : _settings.CatchAllType, _cCurrent.ToString(), _nRow, _nColumn);
				Advance();
				return result;

			}
			return new Token(BuiltinTokenTypes.EndOfFile, BuiltinTokenTypes.EndOfFile, _nRow, _nColumn);
		}

		private bool CompareRule(string a, TokenizerRule rule)
		{
			var stringToMatch = rule.StringToMatch;
			if (rule.IsEnclosed) stringToMatch = rule.EnclosingLeft;
			if (_settings.IgnoreCase || rule.IgnoreCase) return a.ToLower() == stringToMatch.ToLower();
			return a == stringToMatch;
		}
		

		private void Advance(int next)
		{
			for (int i = 0; i < next; i++)
			{
				Advance();
			}
		}
		private void Advance()
		{
			Count();
			_nIndex++;
			if (_nIndex >= _text.Length)
			{
				_bAtEnd = true;
				return;
			}
			_cCurrent = _text[_nIndex];
		}

		private void Count()
		{
			if (_settings.AllOneLine)
			{
				_nColumn++;
				return;
			}
			if (_cCurrent == '\n')
			{
				_nRow++;
				_nColumn = 0;
			}
			else if (_cCurrent == '\r')
			{
				_nColumn = 0;
			}
			else if (_cCurrent == '\t')
			{
				_nColumn += 4;
			}
			else
			{
				_nColumn++;
			}
		}

		private string LookAhead(int peek)
		{
			string result = "";

			for (int i = 0; i < peek; i++)
			{
				if (_nIndex + i < _text.Length)
				{
					result += _text[Convert.ToInt32(_nIndex + i)];
					continue;
				}
				break;
			}
			return result;
		}


		// Multi-Character token processing

		private Token GetStringToken(string enclosing)
		{
			StringBuilder result = new StringBuilder();
			bool bSlash = false;
			while (!_bAtEnd && (LookAhead(enclosing.Length) != enclosing || bSlash))
			{
				if (_cCurrent == '\\' && !bSlash)
				{
					bSlash = true;
					Advance();
					continue;
				}
				if (bSlash)
				{
					if (_cCurrent == 'n')
					{
						result.Append('\n');
						Advance();
					}
					else if (_cCurrent == 't')
					{
						result.Append('\t');
						Advance();
					}
					else if (_cCurrent == 'r')
					{
						result.Append('\r');
						Advance();
					}
					else if (_cCurrent == 'a')
					{
						result.Append('\a');
						Advance();
					}
					else if (_cCurrent == 'b')
					{
						result.Append('\b');
						Advance();
					}
					else if (_cCurrent == 'v')
					{
						result.Append('\v');
						Advance();
					}
					else if (_cCurrent == 'f')
					{
						result.Append('\f');
						Advance();
					}
					else if (_cCurrent == '"')
					{
						result.Append('\"');
						Advance();
					}
					else if (_cCurrent == '\'')
					{
						result.Append('\'');
						Advance();
					}
					else if (_cCurrent == '0')
					{
						result.Append('\0');
						Advance();
					}
					else if (_cCurrent == '\\')
					{
						result.Append('\\');
						Advance();
					}
					else
					{
						result.Append('\\');
						result.Append(_cCurrent);
						Advance();
					}
					bSlash = false;
					continue;
				}
				else
				{
					result.Append(_cCurrent);
					Advance();
					bSlash = false;
				}
			}

			if (!_bAtEnd)
			{
				Advance(enclosing.Length);
			}
			return new Token(BuiltinTokenTypes.String, result.ToString(), _nRow, _nColumn);
		}

		private Token GetEnclosedToken(TokenizerRule rule)
		{
			StringBuilder result = new StringBuilder();
			while (!_bAtEnd && LookAhead(rule.EnclosingRight.Length) != rule.EnclosingRight)
			{
				result.Append(_cCurrent);
				Advance();
			}

			if (!_bAtEnd)
			{
				Advance(rule.EnclosingRight.Length);
			}
			return new Token(rule.Type, result.ToString(), _nRow, _nColumn);
		}

		private Token Word()
		{
			StringBuilder result = new StringBuilder();

			string type = BuiltinTokenTypes.Word;
			while (!_bAtEnd && (_cCurrent == '_' || char.IsLetterOrDigit(_cCurrent) || _cCurrent == '\0' || _settings.WordIncluded.Contains(_cCurrent)))
			{
				if (_cCurrent != '\0')
				{
					result.Append(_cCurrent);
				}
				Advance();
			}

			var stringResult = result.ToString();

            foreach (TokenizerRule rule in _rules)
            {
                if (CompareRule(stringResult, rule))
                {
                    if (rule.Type == BuiltinTokenTypes.EndOfLineComment)
                    {
                        return EndOfLineComment();
                    }

                    if (rule.Type == BuiltinTokenTypes.MultiLineComment)
                    {
                        return GetEnclosedToken(rule);
                    }

                    if (rule.Type == BuiltinTokenTypes.String && rule.IsEnclosed)
                    {
                        return GetStringToken(rule.EnclosingRight);
                    }

                    if (rule.IsEnclosed)
                    {
                        return GetEnclosedToken(rule);
                    }

                    return new Token(rule, _nRow, _nColumn);
                }
            }

            return new Token(type, stringResult, _nRow, _nColumn);
		}

		private Token Number()
		{
			StringBuilder result = new StringBuilder();

			bool bHadDecimal = false;
			bool bIsFloat = false;
			bool bIsDouble = false;
			bool bIsUnsigned = false;
			while (!_bAtEnd && (Char.IsDigit(_cCurrent) || (_cCurrent == '.' && !bHadDecimal) || (_cCurrent == 'f' && bHadDecimal) || (_cCurrent == 'd' && bHadDecimal) || (_cCurrent == 'u' && !bHadDecimal) || (_settings.AllowNegatives && _cCurrent == _settings.NegativeChar && result.Length == 0)))
			{
				if (_cCurrent == '.')
				{
					bHadDecimal = true;
				}
				if (_cCurrent == 'f')
				{
					bIsFloat = true;
					Advance();
					break;
				}
				if (_cCurrent == 'd')
				{
					bIsDouble = true;
					Advance();
					break;
				}
				if (_cCurrent == 'u')
				{
					bIsUnsigned = true;
					Advance();
					break;
				}

				result.Append(_cCurrent);
				Advance();
			}

			string type = BuiltinTokenTypes.Integer;
			if (bHadDecimal)
			{
				if (bIsFloat)
				{
					type = BuiltinTokenTypes.Float;
				}
				else if (bIsDouble)
				{
					type = BuiltinTokenTypes.Double;
				}
				else
				{
					type = BuiltinTokenTypes.Double;
				}
			}
			else if (bIsUnsigned)
			{
				type = BuiltinTokenTypes.UnsignedInteger;
			}
			return new Token(type, result.ToString(), _nRow, _nColumn);
		}


		private Token EndOfLineComment()
		{
			StringBuilder result = new StringBuilder();
			while (!_bAtEnd && _cCurrent != '\n' && _cCurrent != '\r')
			{
				result.Append(_cCurrent);
				Advance();
			}
			return new Token(BuiltinTokenTypes.EndOfLineComment, result.ToString(), _nRow, _nColumn);
		}
	}
}
