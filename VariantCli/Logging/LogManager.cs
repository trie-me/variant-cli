using System.Reactive.Linq;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Variant.Cli.Logging;

public static class LogManager
{
  private static LogEventLevel LogLevel => (Environment.GetEnvironmentVariable("VARIANT_LOG_LEVEL") ?? "") switch
  {
    "Debug" => LogEventLevel.Debug,
    "Error" => LogEventLevel.Error,
    "Fatal" => LogEventLevel.Fatal,
    "Information" => LogEventLevel.Information,
    "Verbose" => LogEventLevel.Verbose,
    "Warning" => LogEventLevel.Warning,
    _ => LogEventLevel.Error,
  };

  private static LoggingLevelSwitch LogLevelSwitch { get; } = new(LogLevel);

  static LogManager()
  {
    // Check every 3 seconds for log level updates
    Observable.Interval(TimeSpan.FromSeconds(5))
      .Select(_ => LogLevel)
      .StartWith(LogLevel)
      .Distinct().Subscribe(
        logLevel =>
        {
          Logger.Information($"LogLevel set to {logLevel}");
          LogLevelSwitch.MinimumLevel = logLevel;
        },
        ex => Logger.Error(ex.Message)
      );
  }

  public static Logger Logger { get; } = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(LogLevelSwitch)
    .WriteTo.Console().MinimumLevel.ControlledBy(LogLevelSwitch)
    .Enrich.FromLogContext()
    .CreateLogger();
}