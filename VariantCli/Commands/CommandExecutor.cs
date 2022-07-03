using System.Reflection;
using CommandLine;
using Variant.Cli.Actions;
using Variant.Cli.Logging;

namespace Variant.Cli.Commands;

public static class CommandExecutor
{
  /// <summary>
  /// Get the defined verbs in the current assembly 
  /// </summary>
  /// <param name="args"></param>
  /// <returns></returns>
  private static Type[] GetCommandTypesFromCurrentAssembly(params string[] args) =>
    Assembly.GetExecutingAssembly()
      .GetTypes()
      .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)
      .ToArray();

  /// <summary>
  /// Execute the configured commands in the application using the provided command line arguments
  /// </summary>
  /// <param name="args"></param>
  public static async Task RunAsync(params string[] args)
  {
    var commandTypes = GetCommandTypesFromCurrentAssembly();
    try
    {
      await Parser.Default.ParseArguments(args, commandTypes)
        .WithParsedAsync(RunMappedCommandActions);
    }
    catch (Exception ex)
    {
      LogManager.Logger.Error(ex.Message);
    }
  }

  /// <summary>
  /// Bind the command action to its underlying command data when present.
  /// </summary>
  /// <param name="resolvedCommand">A command verb resulting from the cli parsing operation</param>
  private static async Task RunMappedCommandActions(object resolvedCommand)
  {
    switch (resolvedCommand)
    {
      case GenerateCommand g:
        await GenerateImageAction
          .Create()
          .ExecuteAsync(g);
        break;
      case InitCommand i:
        await InitConfigAction
          .Create()
          .ExecuteAsync(i);
        break;
    }
  }
}