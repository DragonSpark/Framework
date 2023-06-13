using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly ICommand _refresh;

	public ActiveContent(IResulting<T?> content) : this(content, new Variable<T>(), new Variable<int>()) {}

	public ActiveContent(IResulting<T?> content, IMutable<T?> store, IMutable<int> counts)
		: this(content, new VisitedAwareVariable<T?>(store, counts), counts) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<int> counts)
		: this(new Storing<T?>(store, result).Then().Protecting().Out(), counts) {}

	public ActiveContent(IResulting<T?> result, IMutable<int> counts)
		: this(result, new UpdateMonitor(counts)) {}

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