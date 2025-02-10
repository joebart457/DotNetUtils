
namespace TokenizerCore.Interfaces
{
    public interface IToken
    {
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public ILocation Start { get; set; }
        public ILocation End { get; set; }
    }
}
