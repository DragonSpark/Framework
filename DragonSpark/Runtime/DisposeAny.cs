using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Runtime;

public sealed class DisposeAny : ICommand<object>
{
	public static DisposeAny Default { get; } = new();

	DisposeAny() {}

	public void Execute(object parameter)
	{
		parameter.ToDisposable().Dispose();
	}
}