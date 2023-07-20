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

        public static void ResolveWithTryCatch<Ty>(this string[] args, Action<Exception> callback) where Ty : class
        {
            try
            {
                Resolve<Ty>(args);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        public static void ResolveWithTryCatch<Ty>(this string[] args, Ty instance, Action<Exception> callback) where Ty : class
        {
            try
            {
                Resolve(args, instance);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        internal static CliCommand ParseCommand<Ty>(string[] args) where Ty : class
        {
            Type type = typeof(Ty);
            if (!Attribute.IsDefined(type, typeof(EntryAttribute))) throw new ArgumentException("cli entry class must have Entry(\"exeName\" ) defined");
            var commands = type.GetMethods().Where(m => Attribute.IsDefined(m, typeof(CommandAttribute)));
            var cliCommands = commands.Select(c => new CliCommand(c));
            var resolvable = cliCommands.Where(c => c.Resolvable(args) && c.Path.Count > 0);
            if (!resolvable.Any())
            {
                resolvable = cliCommands.Where(c => c.Resolvable(args) && c.Path.Count == 0);
                if (!resolvable.Any())  throw new CliValidationException($"unable to resolve command {string.Join(' ', args)}");
            }
            if (resolvable.Count() > 1) throw new CliValidationException($"expected only 1 resolvable command but got {resolvable.Count()}");
            return resolvable.First();
        }

        public static string GetHelpText<Ty>()
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
                sb.AppendLine(string.Join('\n', cliCommands.OrderBy(c => c.ToHelpString()).Select(c => c.ToHelpString())));
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
