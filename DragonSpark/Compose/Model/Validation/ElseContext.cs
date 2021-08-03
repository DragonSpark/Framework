using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class ElseContext<T>
	{
		readonly Func<T, bool> _if, _subject;

		public ElseContext(Func<T, bool> @if, Func<T, bool> subject)
		{
			_if      = @if;
			_subject = subject;
		}

		public ConditionSelector<T> Else(Func<T, bool> @else) => new ThenElse<T>(_if, _subject, @else).Then();
	}
}