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
        public async Task DoThis(string strArg, bool flag = false)
        {
            await Task.Run(() => Thread.Sleep(1000));
            Console.Write($"{strArg}: {flag}");
        }
    }
}
