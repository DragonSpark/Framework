using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IActiveContent<T> _previous;
	readonly IResulting<T?>    _source;

	public ActiveContentAdapter(IActiveContent<T> previous, IResulting<T?> source)
		: this(previous, source, previous.Condition) {}

	public ActiveContentAdapter(IActiveContent<T> previous, IResulting<T?> source, ICondition<None> monitor)
	{
		_previous = previous;
		_source   = source;
		Condition  = monitor;
	}

	public ICondition<None> Condition { get; }

	public ValueTask<T?> Get() => _source.Get();

	public ValueTask Get(Action parameter) => _previous.Get(parameter);
}