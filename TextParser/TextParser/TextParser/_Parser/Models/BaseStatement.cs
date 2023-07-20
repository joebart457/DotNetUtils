using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextParser._Parser.Models
{
    public class BaseStatement
    {
        public string Name { get; set; } = "BaseStatement";

        public BaseStatement() { }
        public BaseStatement(string name)
        {
            Name = name;
        }
        virtual public object? Execute()
        {
            return null;
        }
    }
}
