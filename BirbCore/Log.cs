using StardewModdingAPI;

namespace BirbCore
{
    public class Log
    {
        private static IMonitor Monitor;

        public static void Init(IMonitor monitor)
        {
            Monitor = monitor;
        }
        public static void Debug(string str, bool isDebug = true)
        {
            Monitor.Log(str, isDebug ? LogLevel.Debug : LogLevel.Trace);
        }
        public static void Alert(string str)
        {
            Monitor.Log(str, LogLevel.Alert);
        }
        public static void Error(string str)
        {
            Monitor.Log(str, LogLevel.Error);
        }
        public static void Info(string str)
        {
            Monitor.Log(str, LogLevel.Info);
        }
        public static void Trace(string str)
        {
            Monitor.Log(str, LogLevel.Trace);
        }
        public static void Warn(string str)
        {
            Monitor.Log(str, LogLevel.Warn);
        }
    }
}
