using ImageMagick;
using Variant.Cli.Configuration;
using Variant.Cli.Logging;

namespace Variant.Cli.Utilities;

internal static class WebImageUtils
{
  private static ImageOptimizer opt = new()
  {
    OptimalCompression = true,
    IgnoreUnsupportedFormats = false
  };

  public static async Task OptimizeImageForWebAsync(this FileInfo attrs, ImageTransform[] config)
  {
    using var image = new MagickImage(attrs);
    foreach (var imageTransform in SortSizeForAssociativity(config))
    {
      if (IsTransformationInvalidInContext(attrs, image, imageTransform)) continue;

      var outfile = imageTransform.GetOutputFilePath(attrs);
      var targetFile = EnsureDirectoryExists(outfile);

      LogManager.Logger.Information($"Resizing {targetFile.FullName}");
      SetImageWebOptimizationDefaults(image, imageTransform);
      await image.WriteAsync(outfile);
      LogManager.Logger.Information($"Saved {targetFile.FullName}");
      TryOptimizeImageCompression(opt, image, targetFile);
    }
  }

  private static IOrderedEnumerable<ImageTransform> SortSizeForAssociativity(ImageTransform[] config)
  {
    return config.OrderByDescending(v => v.PixelWidth);
  }

  private static bool IsTransformationInvalidInContext(FileInfo attrs, MagickImage image, ImageTransform imageTransform)
  {
    if (image.Width >= imageTransform.PixelWidth) return false;
    LogManager.Logger.Information(
      $"Image {attrs.FullName} is smaller than the requested transform of {imageTransform.PixelWidth}px, skipping.");
    return true;
  }

  private static FileInfo EnsureDirectoryExists(string outfile)
  {
    var targetFile = new FileInfo(outfile);
    if (Directory.Exists(targetFile.DirectoryName)) return targetFile;
    Directory.CreateDirectory(targetFile.DirectoryName);
    return targetFile;
  }

  private static void TryOptimizeImageCompression(ImageOptimizer opt, MagickImage image, FileInfo targetFile)
  {
    if (!opt.IsSupported(image.FormatInfo)) return;
    LogManager.Logger.Debug($"Optimizing {targetFile.FullName}.");
    opt.LosslessCompress(targetFile.FullName);
    LogManager.Logger.Debug($"{targetFile.FullName} optimized.");
  }

  private static void SetImageWebOptimizationDefaults(MagickImage image, ImageTransform imageTransform)
  {
    image.Format = imageTransform.TargetFormat;
    image.Quality = 75;
    image.Resize(imageTransform.PixelWidth, 0);
    image.Density = new Density(72, 72, DensityUnit.PixelsPerInch);
  }
}