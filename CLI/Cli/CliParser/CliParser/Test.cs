using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    class Example 
    {
        public int value;
        public Example(string val)
        {
            value = int.Parse(val);
        }

        public static implicit operator Example(string val) => new Example(val);
    }
    [Entry("exe")]
    internal class Test
    {
        [SubCommand("please")]
        [Command("doThis")]
        public async Task DoThis(string strArg, Example delay, bool flag = false)
        {
            await Task.Run(() => Thread.Sleep(delay.value));
            Console.Write($"{strArg}: {flag}");
        }

        [Command]
        public void NoPathCommand(int testArg)
        {
            Console.WriteLine($"test arg: {testArg}");
        }
    }
}
