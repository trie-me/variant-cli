namespace Variant.Cli.Actions;

public abstract class AbstractCommandAction<TAction, TCommand> : IAsyncCommandAction<TCommand>
  where TAction : IAsyncCommandAction<TCommand>
{
  public static TAction Create()
  {
    return Activator.CreateInstance<TAction>();
  }

  public abstract Task ExecuteAsync(TCommand command);
}