using ImageMagick;
using Variant.Cli.Configuration;

namespace Variant.Cli;

public class Defaults
{
  public static HashSet<string> AllowedExts = new()
  {
    "webp",
    "png",
    "jpg"
  };
  public static ImageTransform[] DefaultWebTransforms
  {
    get
    {
      var transforms = new ImageTransform[]
      {
        new(9000, MagickFormat.WebP, "xxxlarge"),
        new(1920, MagickFormat.WebP, "xxlarge"),
        new(1200, MagickFormat.WebP, "xlarge"),
        new(980, MagickFormat.WebP, "large"),
        new(768, MagickFormat.WebP, "medium"),
        new(540, MagickFormat.WebP, "small"),
        new(480, MagickFormat.WebP, "xsmall"),
        new(200, MagickFormat.WebP, "thumnbnail-large"),
        new(150, MagickFormat.WebP, "thumbnail-medium"),
        new(100, MagickFormat.WebP, "thumbnail-small"),
        new(75, MagickFormat.WebP, "thumbnail-xsmall"),
        new(9000, MagickFormat.Jpg, "xxxlarge"),
        new(1920, MagickFormat.Jpg, "xxlarge"),
        new(1200, MagickFormat.Jpg, "xlarge"),
        new(980, MagickFormat.Jpg, "large"),
        new(768, MagickFormat.Jpg, "medium"),
        new(540, MagickFormat.Jpg, "small"),
        new(480, MagickFormat.Jpg, "xsmall"),
        new(200, MagickFormat.Jpg, "thumnbnail-large"),
        new(150, MagickFormat.Jpg, "thumbnail-medium"),
        new(100, MagickFormat.Jpg, "thumbnail-small"),
        new(75, MagickFormat.Jpg, "thumbnail-xsmall"),
      };
      return transforms;
    }
  }
}