using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    public static class MainExtensions
    {
        public static void Resolve<Ty>(this string[] args) where Ty : class
        {
            try
            {
                CliCommand cliCommand = ParseCommand<Ty>(args);
                cliCommand.Run<Ty>(args);
            }
            catch (CliValidationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(GetHelpText<Ty>());
            }
        }

        public static void Resolve<Ty>(this string[] args, Ty instance) where Ty : class
        {
            try
            {
                CliCommand cliCommand = ParseCommand<Ty>(args);
                cliCommand.Run(args, instance);
            }
            catch (CliValidationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(GetHelpText<Ty>());
            }
        }

        internal static CliCommand ParseCommand<Ty>(string[] args) where Ty : class
        {
            Type type = typeof(Ty);
            if (!Attribute.IsDefined(type, typeof(EntryAttribute))) throw new ArgumentException("cli entry class must have Entry(\"exeName\" ) defined");
            var commands = type.GetMethods().Where(m => Attribute.IsDefined(m, typeof(CommandAttribute)));
            var cliCommands = commands.Select(c => new CliCommand(c));
            var resolvable = cliCommands.Where(c => c.Resolvable(args));
            if (!resolvable.Any()) throw new CliValidationException($"unable to resolve command {string.Join(' ', args)}");
            if (resolvable.Count() > 1) throw new CliValidationException($"expected only 1 resolvable command but got {resolvable.Count()}");
            return resolvable.First();
        }

        internal static string GetHelpText<Ty>()
        {
            try
            {
                Type type = typeof(Ty);
                var sb = new StringBuilder();
                if (type.GetCustomAttribute(typeof(EntryAttribute)) is EntryAttribute entryAttribute)
                {
                    sb.AppendLine($"Usage: {entryAttribute.ExeName} -<options>");
                }
                sb.AppendLine("Options:");
                var commands = type.GetMethods().Where(m => Attribute.IsDefined(m, typeof(CommandAttribute)));
                var cliCommands = commands.Select(c => new CliCommand(c));
                sb.AppendLine(string.Join('\n', cliCommands.OrderBy(c => c.Path).Select(c => c.ToHelpString())));
                return sb.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
        internal static string ToCommandOption(this string src)
        {
            if (src.StartsWith("--")) return src.Substring(2);
            if (src.StartsWith("-")) return src.Substring(1);
            return src;
        }

    }
}
