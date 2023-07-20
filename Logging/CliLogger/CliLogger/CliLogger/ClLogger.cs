using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public enum LogSeverity
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2,
        SUCCESS = 3,
        LOGS = 4,
        LIST = 5,
    }
    public static class CliLogger
    {
        private static Dictionary<LogSeverity, ConsoleColor> _colorSettings = new Dictionary<LogSeverity, ConsoleColor>()
        {
            { LogSeverity.INFO, ConsoleColor.Cyan },
            { LogSeverity.WARNING, ConsoleColor.DarkYellow },
            { LogSeverity.ERROR, ConsoleColor.Red },
            { LogSeverity.SUCCESS, ConsoleColor.Green },
            { LogSeverity.LOGS, ConsoleColor.DarkMagenta },
            { LogSeverity.LIST, ConsoleColor.DarkCyan },
        };

        public static bool LoggingEnabled { get; set; } = true;
        public static void SetColor(LogSeverity severity, ConsoleColor color)
        {
            _colorSettings[severity] = color;
        }
        public static void ResetColorDefaults()
        {
            _colorSettings.Clear();
            _colorSettings[LogSeverity.INFO] = ConsoleColor.Cyan;
            _colorSettings[LogSeverity.WARNING] = ConsoleColor.DarkYellow;
            _colorSettings[LogSeverity.ERROR] = ConsoleColor.Red;
            _colorSettings[LogSeverity.SUCCESS] = ConsoleColor.Green;
            _colorSettings[LogSeverity.LOGS] = ConsoleColor.DarkMagenta;
            _colorSettings[LogSeverity.LIST] = ConsoleColor.DarkCyan;
        }
        public static void Log(string msg, LogSeverity severity = LogSeverity.INFO, bool force = false, string lineEnding = "\r\n")
        {
            if (LoggingEnabled || force) Write(msg, _colorSettings[severity], lineEnding);
        }

        public static void Log<Ty>(List<Ty> ls, bool force = false) where Ty : class
        {
            Log(ls, new List<string>(), force);
        }
        public static void Log<Ty>(List<Ty> ls, List<string> excludeFields, bool force = false) where Ty : class
        {
            if (LoggingEnabled || force)
            {
                if (ls is List<string> data)
                {
                    foreach (var str in data)
                    {
                        Log(str, LogSeverity.LIST);
                    }
                    return;
                }
                var headers = typeof(Ty).GetProperties().Where(p => !excludeFields.Contains(p.Name)).Select(p => p.Name);
                Log(string.Join(" | ", headers), LogSeverity.LIST);
                foreach (var item in ls)
                {
                    var values = typeof(Ty).GetProperties().Where(p => !excludeFields.Contains(p.Name)).Select(prop => $"{prop.GetValue(item)}");
                    Log(string.Join("   ", values), LogSeverity.LIST);
                }
            }
        }

        public static void LogSuccess(string msg)
        {
            Log(msg, LogSeverity.SUCCESS);
        }

        public static void LogWarning(string msg)
        {
            Log(msg, LogSeverity.WARNING);
        }

        public static void LogError(string msg)
        {
            Log(msg, LogSeverity.ERROR);
        }

        public static void LogInfo(string msg)
        {
            Log(msg, LogSeverity.INFO);
        }

        public static void LogLogs(string msg)
        {
            Log(msg, LogSeverity.LOGS);
        }

        private static void Write(string msg, ConsoleColor color, string lineEnding = "\r\n")
        {
            Console.ForegroundColor = color;
            Console.Write($"{msg}{lineEnding}");
            Console.ResetColor();
        }
    }
}
