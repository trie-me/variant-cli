using ImageMagick;

namespace Variant.Cli.Configuration;

public static class ImageTransformExtensions
{
  public static string GetOutputFilePath(this ImageTransform transform, FileInfo attrs) =>
    Path.Combine(attrs.DirectoryName, attrs.Name.Replace(attrs.Extension, ""),
      $"{transform.VariationName ?? transform.PixelWidth + "px"}.{Enum.GetName(transform.TargetFormat)?.ToLower() ?? "FormatNotFound".ToLower()}"
    );
}

public record ImageTransform(int PixelWidth, MagickFormat TargetFormat, string? VariationName = null);

/// <summary>
/// Represents the config file that w
/// </summary>
/// <param name="PixelWidth"></param>
/// <param name="TargetFormat"></param>
public record ImageTransformDTO(int PixelWidth, string TargetFormat, string? VariationName = null);