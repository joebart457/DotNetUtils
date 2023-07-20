using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenizerCore.Interfaces
{
    public interface IToken
    {
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public ILocation Location { get; set; }
    }
}
