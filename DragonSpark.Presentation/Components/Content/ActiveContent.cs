using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly IOperation<Action> _refresh;

	public ActiveContent(IResulting<T?> content) : this(content, new Variable<T>(), new Variable<int>()) {}

	public ActiveContent(IResulting<T?> content, IMutable<T?> store, IMutable<int> counts)
		: this(content, new VisitedAwareVariable<T?>(store, counts), counts) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<int> counts)
		: this(new Storing<T?>(store, result), counts) {}

	public ActiveContent(IResulting<T?> result, IMutable<int> counts)
		: this(result, new UpdateMonitor<T?>(result, counts)) {}

	public ActiveContent(IResulting<T?> result, UpdateMonitor<T?> monitor) : this(result, monitor, monitor) {}

	public ActiveContent(IResulting<T?> result, IOperation<Action> refresh, ICondition monitor) : base(result)
	{
		_refresh  = refresh;
		Condition = monitor;
	}

	public ICondition<None> Condition { get; }

	public ValueTask Get(Action parameter) => _refresh.Get(parameter);
}