using ParsingCoreV2.Implementation;
using ParsingCoreV2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace ParsingCoreV2
{
    public class ParsingEngine
    {
        private readonly IParserInterface _parserInterface;

        public ParsingEngine(IParserInterface parserInterface)
        {
            _parserInterface = parserInterface;
        }

        public ParsingEngine()
        {
            _parserInterface = new ParserInterface();
        }
        public ParsingEngine(IList<IToken> tokens)
        {
            _parserInterface = new ParserInterface();
            _parserInterface.Initialize(tokens);
        }

        public static Ty? ParseOrDefault<Ty>(IParserInterface parser, IParsable<Ty> parsable, Ty? _default = default(Ty))
        {
            if (parsable.CanParse(parser)) return parsable.Parse(parser);
            return _default;
        }

        public static bool TryParse<Ty>(IParserInterface parser, IParsable<Ty> parsable, out Ty? result)
        {
            if (parsable.CanParse(parser))
            {
                result = parsable.Parse(parser);
                return true;
            }
            result = default(Ty);
            return false;
            
        }

        public Ty? ParseOrDefault<Ty>(IParsable<Ty> parsable, Ty? _default = default(Ty))
        {
            return ParseOrDefault(_parserInterface, parsable, _default);
        }

        public bool TryParse<Ty>(IParsable<Ty> parsable, out Ty? result)
        {
            return TryParse(_parserInterface, parsable, out result);
        }
    }
}
