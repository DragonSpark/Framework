using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class ValidatedResultContext<T>
	{
		readonly IResult<T> _other;
		readonly IResult<T> _subject;

		public ValidatedResultContext(IResult<T> subject, IResult<T> other)
		{
			_subject = subject;
			_other   = other;
		}

		public ResultContext<T> IsAssigned() => Is(IsAssigned<T>.Default);

		public ResultContext<T> Is(Func<T, bool> condition) => Is(Start.A.Condition(condition).Out());

		public ResultContext<T> Is(ICondition<T> condition)
			=> new ValidatedResult<T>(condition, _other, _subject).Then();

		public ResultContext<T> Is(ICondition condition) => Is(condition.Get);

		public ResultContext<T> Is(Func<bool> condition)
			=> new Validated<T>(condition, _other.Get, _subject.Get).Then();
	}
}