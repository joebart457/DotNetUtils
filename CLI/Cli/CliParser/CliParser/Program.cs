
using CliParser;

internal class Program
{
	static void Main(string[] args)	{
		var t = new Test();
		new string[] {"please", "doThis", "-f", "--strArg", "Hello world"}.Resolve(t);

	}
}