using ParsingCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingCore.Tests.Models
{
    internal class LocationTm: ILocation
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
