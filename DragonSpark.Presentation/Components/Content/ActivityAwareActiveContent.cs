using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.State;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly IActiveContent<T> _previous;

	public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
		: this(previous, previous.Condition, new ActivityAwareResult<T>(previous, receiver)) {}

	public ActivityAwareActiveContent(IActiveContent<T> previous, ICondition<None> refresh, IResulting<T?> resulting)
		: base(resulting)
	{
		_previous = previous;
		Condition   = refresh;
	}

	public ICondition<None> Condition { get; }

	public ValueTask Get(Action parameter) => _previous.Get(parameter);
}