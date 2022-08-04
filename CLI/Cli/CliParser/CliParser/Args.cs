using System;
using System.Collections.Generic;
using System.Linq;
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

            _lookup.Add(key, Convert.ChangeType(value, commandParameter.ParameterType));
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
