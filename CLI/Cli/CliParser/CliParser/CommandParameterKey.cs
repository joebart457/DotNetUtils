using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    internal class CommandParameterKey
    {
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public string Description { get; set; } = "";
        public int Position { get; set; }
        public bool IsRequired { get; set; }
        public bool HasDefault { get; set; }
        public object? DefaultValue { get; set; }
        public Type ParameterType { get; set; } = typeof(string);

        public CommandParameterKey() { }
        public CommandParameterKey(CommandParameter commandParameter)
        {
            Name = commandParameter.Name;
            Abbreviation = commandParameter.Abbreviation;
            Position = commandParameter.Position;
            Description = commandParameter.Description;
            IsRequired = commandParameter.IsRequired;
            HasDefault = commandParameter.HasDefault;
            ParameterType = commandParameter.ParameterType;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            if (obj is CommandParameterKey commandParameterKey)
            {
                return Name == commandParameterKey.Name ||
                    Abbreviation == commandParameterKey.Abbreviation ||
                    Position == commandParameterKey.Position;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Abbreviation, Position);
        }
    }
}
