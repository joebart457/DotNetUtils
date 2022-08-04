using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliParser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntryAttribute : System.Attribute
    {
        public string ExeName { get; set; } = "";
        public EntryAttribute(string exeName)
        {
            ExeName = exeName;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : System.Attribute
    {
        public string Verb { get; set; } = "";
        public CommandAttribute(string verb)
        {
            Verb = verb;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SubCommandAttribute : System.Attribute
    {
        public string Verb { get; set; } = "";
        public SubCommandAttribute(string verb)
        {
            Verb = verb;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class OptionAttribute : System.Attribute
    {
        public string Abbr { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public OptionAttribute()
        {
        }
    }
}
