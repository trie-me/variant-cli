using Variant.Cli.Commands;
using Variant.Cli.Configuration;
using Variant.Cli.Logging;
using Variant.Cli.Utilities;

namespace Variant.Cli.Actions;

public class GenerateImageAction : AbstractCommandAction<GenerateImageAction, GenerateCommand>
{
  public override async Task ExecuteAsync(GenerateCommand command)
  {
    FileInfo[]? files = Directory.Exists(command.Path)
      ? FileUtils.GetDirectoryFileModeImages(command.Path)
      : FileUtils.GetSingleFileModeImage(command.Path);

    ImageTransform[]? config = ValidateImageTransform(command);
    if (config is not null)
      await Parallel.ForEachAsync(files, new ParallelOptions
      {
        MaxDegreeOfParallelism = command.MaxThreads
      }, async (info, _) => await info.OptimizeImageForWebAsync(config));
  }

  private ImageTransform[]? ValidateImageTransform(GenerateCommand command)
  {
    var config = ImageConfigurationResolver.GetImageTransformConfig(command.TransformConfig, command.Path);
    if (config is not null) return config;
    LogManager.Logger.Information(
      "Configuration could not be resolved with given values. Please check your configuration and try again.");
    return null;
  }
  
}