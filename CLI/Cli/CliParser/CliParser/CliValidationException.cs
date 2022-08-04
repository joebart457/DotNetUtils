using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    public class CliValidationException : Exception
    {
        public CliValidationException(string msg)
            : base(msg)
        {


        }
    }
}
