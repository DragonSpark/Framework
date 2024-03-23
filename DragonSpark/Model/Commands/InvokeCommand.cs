using JetBrains.Annotations;
using System;

namespace DragonSpark.Model.Commands;

[UsedImplicitly]
sealed class InvokeCommand<T> : ICommand<None>
{
	readonly Func<T> _delegate;

	public InvokeCommand(Func<T> @delegate) => _delegate = @delegate;

	public void Execute(None _)
	{
		_delegate();
	}
}