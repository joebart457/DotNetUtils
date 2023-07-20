using ParsingCore.Interfaces;
using ParsingCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace ParsingCore
{
    public class ParsingEngine<Ty>
    {
        private IList<(Func<IParserInterface, bool> condition, Func<IParserInterface, Ty> rule)> _rules = new List<(Func<IParserInterface, bool> condition, Func<IParserInterface, Ty> rule)>();

        public ParsingEngine()
        {
        }

        public IEnumerable<Ty> Parse(IList<IToken> tokens, int offset = 0)
        {
            var parserInterface = new ParserInterface();
            parserInterface.Initialize(tokens, offset);
            if (parserInterface.AtEnd()) yield break;

            var previous = parserInterface.Current();
            do
            {
                if (parserInterface.AtEnd()) yield break;
                foreach ((var condition, var rule) in _rules)
                {
                    if (!condition(parserInterface)) continue;
                    yield return rule(parserInterface);
                    break;
                }
            } while (previous != parserInterface.Current());
        }

        public IEnumerable<Ty> Parse(IParserInterface parserInterface)
        {
            if (parserInterface.AtEnd()) yield break;

            var previous = parserInterface.Current();
            do
            {
                if (parserInterface.AtEnd()) yield break;
                foreach ((var condition, var rule) in _rules)
                {
                    if (!condition(parserInterface)) continue;
                    yield return rule(parserInterface);
                    break;
                }
            } while (previous != parserInterface.Current());
        }

        public IEnumerable<Ty> Parse(IParserInterface parserInterface, IList<IToken> tokens, int offset = 0)
        {
            parserInterface.Initialize(tokens, offset);
            if (parserInterface.AtEnd()) yield break;

            var previous = parserInterface.Current();
            do
            {
                if (parserInterface.AtEnd()) yield break;
                foreach ((var condition, var rule) in _rules)
                {
                    if (!condition(parserInterface)) continue;
                    yield return rule(parserInterface);
                    break;
                }
            } while (previous != parserInterface.Current());
        }

        public Ty? ParseSingleOrDefault(IList<IToken> tokens, int offset = 0)
        {
            var parserInterface = new ParserInterface();
            parserInterface.Initialize(tokens, offset);
            if (parserInterface.AtEnd()) return default(Ty);

            foreach ((var condition, var rule) in _rules)
            {
                if (!condition(parserInterface)) continue;
                return rule(parserInterface);
            }
            return default(Ty);
        }

        public Ty? ParseSingleOrDefault(IParserInterface parserInterface)
        {
            if (parserInterface.AtEnd()) return default(Ty);

            foreach ((var condition, var rule) in _rules)
            {
                if (!condition(parserInterface)) continue;
                return rule(parserInterface);
            }
            return default(Ty);
        }

        public Ty? ParseSingleOrDefault(IParserInterface parserInterface, IList<IToken> tokens, int offset = 0)
        {
            parserInterface.Initialize(tokens, offset);
            if (parserInterface.AtEnd()) return default(Ty);

            foreach ((var condition, var rule) in _rules)
            {
                if (!condition(parserInterface)) continue;
                return rule(parserInterface);
            }
            return default(Ty);
        }


        public void AddRule(Func<IParserInterface, bool> ruleCondition, Func<IParserInterface, Ty> rule)
        {
            _rules.Add((ruleCondition, rule));
        }
    }
}
