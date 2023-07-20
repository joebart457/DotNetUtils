// See https://aka.ms/new-console-template for more information
using TextParser._Parser;
using TextParser._Parser.Models;
using TextParser._Tokenizer.Constants;
using TextParser._Tokenizer.Models;

Console.WriteLine("Hello, World!");

var parser = new Parser(new List<TokenizerRule>());

parser.AddRule(parser =>
{
    if (parser.match(".$")) return new BaseStatement("call");
    return null;
});
parser.AddRule(parser =>
{
    if (parser.match("$"))
    {
        var identifier = parser.consume(TokenTypes.TTWord, "expect identifier");
        return new BaseStatement($"value: {identifier}");
    }
    return null;
});
