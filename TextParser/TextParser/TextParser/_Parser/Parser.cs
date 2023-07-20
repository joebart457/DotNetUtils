using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextParser._Parser.Helpers;
using TextParser._Parser.Models;
using TextParser._Tokenizer;
using TextParser._Tokenizer.Models;

namespace TextParser._Parser
{
    internal class Parser: ParsingHelper
    {
        public Dictionary<string, Func<Parser, BaseStatement?>> Rules { get; set; } = new Dictionary<string, Func<Parser, BaseStatement?>>();
        private readonly Tokenizer _tokenizer;
        
        public Parser(IEnumerable<TokenizerRule> rules)
        {
            _tokenizer = new Tokenizer(rules);
        }
        public void AddRule(string name, Func<Parser, BaseStatement?> rule)
        {
            Rules.Add(name, rule);
        }

        public List<BaseStatement> Parse(string rule, string text)
        {
            var tokens = _tokenizer.Tokenize(text);
            init(tokens);

            var statements = new List<BaseStatement>();
            if (Rules.TryGetValue(rule, out var func))
            {

            }
            return statements;
        }

    }
}
