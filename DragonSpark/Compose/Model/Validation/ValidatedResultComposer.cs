using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class ValidatedResultComposer<T>
{
	readonly IResult<T> _other;
	readonly IResult<T> _subject;

	public ValidatedResultComposer(IResult<T> subject, IResult<T> other)
	{
		_subject = subject;
		_other   = other;
	}

	public ResultComposer<T> IsAssigned() => Is(Runtime.IsAssigned<T>.Default);

	public ResultComposer<T> Is(Func<T, bool> condition) => Is(Start.A.Condition(condition).Out());

	public ResultComposer<T> Is(ICondition<T> condition)
		=> new ValidatedResult<T>(condition, _other, _subject).Then();

	public ResultComposer<T> Is(ICondition condition) => Is(condition.Get);

	public ResultComposer<T> Is(Func<bool> condition)
		=> new Validated<T>(condition, _other.Get, _subject.Get).Then();
}