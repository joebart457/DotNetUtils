using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    internal class CommandParameter
    {
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public string Description { get; set; } = "";
        public int Position { get; set; }
        public bool IsRequired { get; set; }
        public bool HasDefault { get; set; }
        public object? DefaultValue { get; set; }
        public Type ParameterType { get; set; } = typeof(string);

        public override string ToString()
        {
            return $"({Position}) -{Abbreviation} | --{Name}";
        }

        public string ToHelpString()
        {
            return $" -{Abbreviation} | --{Name} {ParameterType.Name} {(IsRequired ? "*Required*" : "")}: {Description}";
        }
    }
}
