using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    [Entry("exe")]
    internal class Test
    {
        [SubCommand("please")]
        [Command("doThis")]
        public void DoThis(string strArg, bool flag = false)
        {
            Console.Write($"{strArg}: {flag}");
        }
    }
}
