using Serilog;
using Serilog.Core;

namespace Rabbit_core.Log
{
    /// <summary>
    /// 引擎日志类
    /// </summary>
    public static class RaLog
    {
        // 日志输出模板
        private const string CoreLogFormat = @"[Engine] {Timestamp:HH:mm:dd} [{Level:u5}] {Message:lj}{NewLine}";
        private const string ClientLogFormat = @"[Client] {Timestamp:HH:mm:dd} [{Level:u5}] {Message:lj}{NewLine}";

        private static readonly Logger CoreLogger;
        private static readonly Logger ClientLogger;

        static RaLog()
        {
            CoreLogger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console(outputTemplate: CoreLogFormat).CreateLogger();
            ClientLogger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console(outputTemplate: ClientLogFormat).CreateLogger();
        }

        public static void DebugLogCore(string message) => CoreLogger.Debug(message);
        public static void InfoLogCore(string message) => CoreLogger.Information(message);
        public static void ErrorLogCore(string message) => CoreLogger.Error(message);
        public static void FatalLogCore(string message) => CoreLogger.Fatal(message);

        public static void DebugLogClient(string message) => ClientLogger.Debug(message);
        public static void InfoLogClient(string message) => ClientLogger.Information(message);
        public static void ErrorLogClient(string message) => ClientLogger.Error(message);
        public static void FatalLogClient(string message) => ClientLogger.Fatal(message);
    }
}
