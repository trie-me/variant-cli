using CommandLine;

namespace Variant.Cli.Commands;

[Verb("generate", HelpText = "Generate Images from Configuration")]
public class GenerateCommand
{
  [Option(HelpText = "Path to the directory to scan, or a specific file.", Default = "./", Required = false)]
  public string Path { get; init; }

  [Option(HelpText = "MaxThreads to use for conversion work.", Default = 4, Required = false)]
  public int MaxThreads { get; init; }

  [Option(HelpText = "Override output directory. If not present output is assumed to be current location.", Required = false)]
  public string? Output { get; init; }

  [Option(HelpText =
      "Specifies the name of the file containing the transform config. If no file is specified, it will look for configuration in .imagetransform",
    Default = ".imagetransform", Required = false)]
  public string? TransformConfig { get; init; }
}