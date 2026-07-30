using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly ICommand _refresh;

	public ActiveContent(IResulting<T?> content) : this(content, new Variable<T>(), new Switch()) {}

	public ActiveContent(IResulting<T?> content, IMutable<T?> store, IMutable<bool> state)
		: this(content, new VisitedAwareVariable<T?>(store, state), state) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<bool> state)
		: this(new Storing<T?>(store, result).Then().Protecting().Out(), state) {}

	public ActiveContent(IResulting<T?> result, IMutable<bool> state) : this(result, new UpdateMonitor(state)) {}

	public ActiveContent(IResulting<T?> result, UpdateMonitor monitor) : this(result, monitor, monitor) {}

	public ActiveContent(IResulting<T?> result, ICommand refresh, ICondition monitor) : base(result)
	{
		_refresh  = refresh;
		Condition = monitor;
	}

	public ICondition<None> Condition { get; }

	public void Execute(None parameter)
	{
		_refresh.Execute(parameter);
	}
}