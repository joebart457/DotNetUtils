using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenizerCore.Interfaces
{
    public interface ILocation
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
