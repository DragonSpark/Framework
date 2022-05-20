using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class AdjustToClientTime : IAdjustToClientTime
{
	readonly ClientTimeOffsetStore _offset;
	readonly ICondition<None>      _ready;

	public AdjustToClientTime(ClientTimeOffsetStore offset) : this(offset, offset.Condition) {}

	public AdjustToClientTime(ClientTimeOffsetStore offset, ICondition<None> ready)
	{
		_offset = offset;
		_ready  = ready;
	}

	public DateTimeOffset? Get(DateTimeOffset parameter)
		=> _offset.IsSatisfiedBy() ? parameter.ToOffset(_offset.Get()) : null;

	public bool Get(None parameter) => _ready.Get(parameter);
}