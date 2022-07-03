namespace Variant.Cli.Actions;

public interface IAsyncCommandAction<in T>
{
  Task ExecuteAsync(T command);
}