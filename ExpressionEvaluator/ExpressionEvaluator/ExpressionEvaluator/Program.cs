// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var e = new ExpressionEvaluator.EvaluatorEngine();

e.RegisterCustomAction((int a, int b)
    => { return a + b; });