using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

[UsedImplicitly]
public class Run : ICommand
{
	readonly Func<Task> _previous;

	protected Run(IOperation previous) : this(previous.Allocate) {}

	protected Run(Func<Task> previous) => _previous = previous;

	public void Execute(None parameter)
	{
		Task.Run(_previous);
	}
}