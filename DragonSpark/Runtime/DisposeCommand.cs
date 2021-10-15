using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Runtime;

sealed class DisposeCommand : Command<IDisposable>
{
	public static ICommand<IDisposable> Default { get; } = new DisposeCommand();

	DisposeCommand() : base(x => x.Dispose()) {}
}