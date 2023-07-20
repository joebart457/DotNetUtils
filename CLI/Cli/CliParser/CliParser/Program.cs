
using CliParser;

internal class Program
{
	static void Main(string[] args)	{
		var t = new Test();
		new string[] {"doThis", "please", "-d", "100", "-f", "--strArg", "Hello world"}.Resolve(t);
        new string[] { "54" }.Resolve(t);

    }
}