using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    internal class CliCommand
    {
        private Dictionary<string, CommandParameter> ParametersByName { get; set; } = new Dictionary<string, CommandParameter>();
        private Dictionary<int, CommandParameter> ParametersByPosition { get; set; } = new Dictionary<int, CommandParameter>();
        public List<string> Path { get; set; }
        private MethodInfo _methodInfo;

        public CliCommand(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            _methodInfo = methodInfo;
            Path = GetPath(methodInfo);
            var parameters = GetParameters(methodInfo);
            parameters.ForEach(p =>
            {
                if (ParametersByName.ContainsKey(p.Name)) throw new CliValidationException($"Internal redefinition of option {p}");
                if (ParametersByName.ContainsKey(p.Abbreviation)) throw new CliValidationException($"Internal redefinition of option {p}");
                if (ParametersByPosition.ContainsKey(p.Position)) throw new CliValidationException($"Internal redifition of positional option {p}");
                ParametersByPosition.Add(p.Position, p);
                ParametersByName.Add(p.Name, p);
                ParametersByName.Add(p.Abbreviation, p);
            });
        }

        public void Run<Ty>(string[] args)
        {
            var arguments = ParseArguments(args);
            var result = _methodInfo.Invoke(typeof(Ty), arguments.ToArgumentArray());
            if (result is Task task) task.Wait();
        }

        public void Run<Ty>(string[] args, Ty instance) where Ty : class
        {
            var arguments = ParseArguments(args);
            var result = _methodInfo.Invoke(instance, arguments.ToArgumentArray());
            if (result is Task task) task.Wait();
        }

        public Args ParseArguments(string[] args)
        {
            if (!Resolvable(args)) throw new ArgumentException("cannot invoke unresolvable command");

            var commandOptions = args.Skip(Path.Count).ToList();

            var resolvedArguments = new Args();
            bool hadExplicitOption = false;
            CommandParameter? previousParameter = null;
            for (int i = 0; i < commandOptions.Count(); i++)
            {
                if (hadExplicitOption)
                {
                    // neccessary for flag options
                    if (previousParameter?.ParameterType == typeof(bool))
                    {
                        if (bool.TryParse(commandOptions[i], out var flag))
                        {
                            resolvedArguments.Add(previousParameter, flag);
                            previousParameter = null;
                            continue;

                        }
                        else
                        {
                            resolvedArguments.Add(previousParameter, true);
                            previousParameter = null;
                        }

                    }

                    if (previousParameter == null)
                    {
                        if (!commandOptions[i].StartsWith("-")) throw new CliValidationException($"expected explicit option to be specified but got {commandOptions[i]}");
                        if (!ParametersByName.TryGetValue(commandOptions[i].ToCommandOption(), out var parameter) || parameter == null) throw new CliValidationException($"option {commandOptions[i]} is not a valid option");
                        previousParameter = parameter;
                        continue;
                    }

                    resolvedArguments.Add(previousParameter, commandOptions[i]);
                    previousParameter = null;
                }
                else
                {
                    if (commandOptions[i].StartsWith("-"))
                    {
                        hadExplicitOption = true;
                        if (!ParametersByName.TryGetValue(commandOptions[i].ToCommandOption(), out var explicitParameter) || explicitParameter == null) throw new CliValidationException($"option {commandOptions[i]} is not a valid option");
                        previousParameter = explicitParameter;
                        continue;
                    }
                    if (!ParametersByPosition.TryGetValue(i, out var parameter) || parameter == null) throw new CliValidationException($"option {commandOptions[i]} is not a valid option");
                    resolvedArguments.Add(parameter, commandOptions[i]);
                }
            }
            if (previousParameter?.ParameterType == typeof(bool))
            {
                resolvedArguments.Add(previousParameter, true);
                previousParameter = null;
            }
            if (previousParameter != null) throw new CliValidationException($"parameter {previousParameter} requires a value to be specified");

            // fill in remaining values with defaults
            ParametersByPosition.Values.Where(p => p.HasDefault && !resolvedArguments.ContainsKey(new CommandParameterKey(p)))
                .ToList().ForEach(p => resolvedArguments.Add(p, p.DefaultValue));

            // check if there are any remaining required parameters
            var remainingRequiredParams = ParametersByPosition.Values.Where(p => p.IsRequired && !p.HasDefault && !resolvedArguments.ContainsKey(new CommandParameterKey(p)));
            if (remainingRequiredParams.Any())
                throw new CliValidationException($"the following parameters are required and must be specified: {string.Join(',', remainingRequiredParams)}");
            return resolvedArguments;
        }
        public bool Resolvable(string[] args)
        {
            if (args.Length < Path.Count) return false;
            for (int i = 0; i < Path.Count; i++)
            {
                if (Path[i] != args[i]) return false;
            }

            return true;
        }

        public string ToHelpString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{string.Join(' ', Path)}");
            sb.AppendLine(string.Join("", ParametersByPosition.Values.Select(p => $"\n    {p.ToHelpString()}")));
            return sb.ToString();
        }
        private static List<string> GetPath(MethodInfo methodInfo)
        {
            var path = new List<string>();
            var attributes = methodInfo.GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                if (attribute is CommandAttribute cmd)
                {
                    if (!string.IsNullOrWhiteSpace(cmd.Verb))
                        path.Insert(0, cmd.Verb);
                }
                if (attribute is SubCommandAttribute subCmd)
                {
                    path.Add(subCmd.Verb);
                }
            }
            return path;
        }
        private static List<CommandParameter> GetParameters(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            var results = new List<CommandParameter>();
            var parameters = methodInfo.GetParameters();
            foreach (var parameter in parameters)
            {
                var cmdParam = new CommandParameter();
                cmdParam.ParameterType = parameter.ParameterType;
                cmdParam.Position = parameter.Position;
                cmdParam.Name = parameter.Name ?? "<unnamed-parameter>";
                cmdParam.Abbreviation = parameter.Name?.FirstOrDefault().ToString() ?? "";
                cmdParam.IsRequired = !parameter.HasDefaultValue;
                cmdParam.HasDefault = parameter.HasDefaultValue;
                cmdParam.DefaultValue = parameter.DefaultValue ?? null;

                var parameterAttrs = parameter.GetCustomAttributes(false);
                foreach (var attribute in parameterAttrs)
                {
                    if (attribute is OptionAttribute option)
                    {
                        cmdParam.Abbreviation = option.Abbr ?? cmdParam.Abbreviation;
                        cmdParam.Name = option.Name ?? cmdParam.Name;
                        cmdParam.Description = option.Description ?? cmdParam.Description;
                        break;
                    }
                }
                results.Add(cmdParam);
            }
            return results;
        }

    }
}
