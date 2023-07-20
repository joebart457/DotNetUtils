using CliParser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    internal class Args
    {
        private Dictionary<CommandParameterKey, object> _lookup = new Dictionary<CommandParameterKey, object>();
        public Args() { }

        public void Add(CommandParameter commandParameter, string value)
        {
            var key = new CommandParameterKey(commandParameter);
            if (_lookup.ContainsKey(key)) throw new CliValidationException($"option {commandParameter} was already provided");

            object tyVal;
            if (commandParameter.ParameterType.HasImplicitConversion(typeof(string)))
            {
                var conversion = commandParameter.ParameterType.GetImplicitConversion(typeof(string));
                if (conversion == null) throw new CliValidationException($"value {value} is not convertible to type {commandParameter.ParameterType.Name}");
                tyVal = conversion.Invoke(commandParameter.ParameterType, new object[] { value }) ?? throw new CliValidationException($"conversion method on type {commandParameter.ParameterType.Name} returned null value");
            }else
            {
                tyVal = Convert.ChangeType(value, commandParameter.ParameterType);
            }
            _lookup.Add(key, tyVal);
        }

        public void Add(CommandParameter commandParameter, object? value)
        {
            var key = new CommandParameterKey(commandParameter);
            if (_lookup.ContainsKey(key)) throw new CliValidationException($"option {commandParameter} was already provided");

            _lookup.Add(key, value);
        }

        public bool ContainsKey(CommandParameterKey key)
        {
            return _lookup.ContainsKey(key);
        }

        public object[] ToArgumentArray()
        {
            return _lookup.ToList().OrderBy(kvp => kvp.Key.Position).Select(kvp => kvp.Value).ToArray();
        }

    }
}
