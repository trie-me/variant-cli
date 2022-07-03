using System.Diagnostics.Contracts;
using Variant.Cli.Logging;

namespace Variant.Cli.Utilities;

public static class FileUtils
{
  [Pure]
  public static FileInfo[]? GetSingleFileModeImage(string Path)
  {
    var imageFile = FileUtils.TryGetFileInfo(Path, out var gotAttrs);
    if (imageFile is null)
    {
      LogManager.Logger.Debug("File information is invalid.");
    }

    return imageFile != null ? new[] { imageFile } : null;
  }

  [Pure]
  public static byte[]? TryGetFile(string path, out bool success)
  {
    var output = (File.Exists(path), Path.GetFullPath(path)) switch
    {
      (false, _) => null,
      var (_, p) => File.ReadAllBytes(p)
    };
    success = output switch
    {
      { } => true,
      _ => false
    };
    return output;
  }

  [Pure]
  public static FileInfo? TryGetFileInfo(string path, out bool success)
  {
    FileInfo? output = (File.Exists(path), Path.GetFullPath(path)) switch
    {
      (false, _) => null,
      var (_, p) => new FileInfo(p)
    };
    success = output switch
    {
      { } => true,
      _ => false
    };
    return output;
  }

  [Pure]
  public static FileInfo[]? GetDirectoryFileModeImages(string Path)
  {
    var result = Directory
      .GetFiles(Path)
      .Select(filePath => new FileInfo(filePath))
      .Where(
        info => Defaults.AllowedExts.Contains(
          info.Extension.ToLower().Replace(".", "")
        )
      );

    return result.ToArray();
  }

  [Pure]
  public static FileInfo? GetImageFileInfo(string Path)
  {
    var imageFile = FileUtils.TryGetFileInfo(Path, out var gotAttrs);
    return imageFile;
  }
}