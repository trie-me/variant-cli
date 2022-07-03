using Newtonsoft.Json;
using Variant.Cli.Commands;
using Variant.Cli.Configuration;
using Variant.Cli.Logging;

namespace Variant.Cli.Actions;

public class InitConfigAction : AbstractCommandAction<InitConfigAction, InitCommand>
{
  public override async Task ExecuteAsync(InitCommand command)
  {
    var defaultFile = JsonConvert.SerializeObject(Defaults.DefaultWebTransforms.Select(
      transform =>
        new ImageTransformDTO(
          transform.PixelWidth,
          Enum.GetName(transform.TargetFormat),
          transform.VariationName
        )
    ));
    var hereFile = "./.imagetransform";
    if (!File.Exists(hereFile))
    {
      LogManager.Logger.Information($"Created init config file at {hereFile}");
      await File.WriteAllTextAsync(hereFile, defaultFile);
    }
    else
      LogManager.Logger.Information($"Configuration file already exists at {hereFile}");
  }
}