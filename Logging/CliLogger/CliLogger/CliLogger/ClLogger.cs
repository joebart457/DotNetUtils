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
        LIST,
    }
    public static class CliLogger
    {
        public static bool LoggingEnabled { get; set; } = true;

        public static void Log(string msg, LogSeverity severity = LogSeverity.INFO, bool force = false)
        {
            if (LoggingEnabled || force)
            {
                switch (severity)
                {
                    case LogSeverity.INFO:
                        Write(msg, ConsoleColor.Cyan);
                        break;
                    case LogSeverity.WARNING:
                        Write(msg, ConsoleColor.DarkYellow);
                        break;
                    case LogSeverity.ERROR:
                        Write(msg, ConsoleColor.Red);
                        break;
                    case LogSeverity.SUCCESS:
                        Write(msg, ConsoleColor.Green);
                        break;
                    case LogSeverity.LOGS:
                        Write(msg, ConsoleColor.DarkMagenta);
                        break;
                    case LogSeverity.LIST:
                        Write(msg, ConsoleColor.DarkCyan);
                        break;
                    default:
                        Write(msg, ConsoleColor.Cyan);
                        break;
                }
            }
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


        private static void Write(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
