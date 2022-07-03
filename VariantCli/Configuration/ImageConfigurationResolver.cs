using ImageMagick;
using Variant.Cli.Logging;

namespace Variant.Cli.Configuration;

public static class ImageConfigurationResolver
{
  private static HashSet<string> SupportedFormats = new()
  {
    "WebP",
    "Png",
    "Jpg"
  };


  public static readonly string CONFIG_FILE_DEFAULT = ".imagetransform";

  public static ImageTransform[]? GetImageTransformConfig(string? configFile, string? path)
  {
    switch (File.Exists(path), Directory.Exists(path))
    {
      case (false, false):
        LogManager.Logger.Debug($"Path spec invalid: {path}");
        return null;
      case (true, _):
        return GetConfigFromFileSpec(configFile, path);
      default: // is directory
        return GetConfigFromDirectorySpec(configFile, path);
    }
  }

  private static ImageTransform[]? GetConfigFromDirectorySpec(string? configFile, string path)
  {
    var dir = new DirectoryInfo(path);
    var configPathFromDir = Path.Combine(dir.FullName, configFile ?? CONFIG_FILE_DEFAULT);
    if (!File.Exists(configPathFromDir))
    {
      LogManager.Logger.Debug($"Could not retrieve a valid config file from {configFile}");
      return null;
    }

    return GetImageTransformConfigFromPath(configPathFromDir);
  }


  private static ImageTransform[]? GetConfigFromFileSpec(string? configFile, string path)
  {
    var file = new FileInfo(path);
    var configPathFromFile = Path.Combine(file.Directory.FullName, configFile ?? CONFIG_FILE_DEFAULT);
    if (!File.Exists(configPathFromFile))
    {
      LogManager.Logger.Debug($"Could not retrieve a valid config file from {configFile}");
      return null;
    }

    return GetImageTransformConfigFromPath(configPathFromFile);
  }

  private static ImageTransform[]? GetImageTransformConfigFromPath(string path)
  {
    var fileData = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageTransformDTO[]>(
      File.ReadAllText(path)
    );
    if (fileData.Any(imageTransformDto => IsTargetFormatInvalid(imageTransformDto)))
      return null;

    return fileData?
      .Select(d =>
        new ImageTransform(
          d.PixelWidth,
          Enum.Parse<MagickFormat>(d.TargetFormat),
          d.VariationName
        ))
      .ToArray();
  }

  private static bool IsTargetFormatInvalid(ImageTransformDTO imageTransformDto)
  {
    if (SupportedFormats.Contains(imageTransformDto.TargetFormat)) return false;
    LogManager.Logger.Debug(
      $"Unsupported Target Format In Config: {imageTransformDto.TargetFormat}. Please review --help for supported types.");
    return true;
  }
}