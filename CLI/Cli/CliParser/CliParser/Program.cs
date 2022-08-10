
using CliParser;

internal class Program
{
	static void Main(string[] args)	{
		var t = new Test();
		new string[] {"doThis", "please", "-f", "--strArg", "Hello world"}.Resolve(t);

	}
}