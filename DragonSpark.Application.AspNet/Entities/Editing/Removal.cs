using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Removal<TIn, T> : StopAware<TIn> where T : class
{
	protected Removal(IStopAware<TIn, T?> select, Remove<T> remove)
		: this(@select, remove, EmptyCommand<TIn>.Default) {}

	protected Removal(IStopAware<TIn, T?> select, Remove<T> remove, ICommand<TIn> command)
		: base(new RemovalDispatch<TIn, T>(select, remove, command)) {}
}