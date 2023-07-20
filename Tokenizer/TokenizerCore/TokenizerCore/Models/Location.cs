using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace TokenizerCore.Model
{
    internal class Location: ILocation
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"Ln. {Line} Col. {Column}";
        }
    }
}
