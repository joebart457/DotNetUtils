using ParsingCore.Tests.Models;
using ParsingCore.Tests.Models.Expressions;
using ParsingCore.Tests.Models.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests
{
    internal static class ParsingEngineTests
    {
        public static void Test()
        {
            var expressionEngine = new ParsingEngine<BaseExpression>();
            expressionEngine.AddRule(
                parser =>
                {
                    return parser.Match(".literal");
                },
                parser =>
                {
                    if (parser.Match("true")) return new Literal(true);
                    else return new Literal(false);
                });



            var engine = new ParsingEngine<BaseStatement>();
            engine.AddRule((parser) =>
            {
                return parser.Match("while");
            },
            (parser) =>
            {
                parser.Consume("(", "expect ( in while");
                var condition = expressionEngine.ParseSingleOrDefault(parser.GetTokens(), parser.Index());
                parser.Consume(")", "expect ) after condition in while");
                var body = engine.ParseSingleOrDefault(parser);
                return new WhileLoop();
            });

        }

    }
}
