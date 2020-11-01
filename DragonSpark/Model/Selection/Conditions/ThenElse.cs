using System;

namespace DragonSpark.Model.Selection.Conditions
{
	sealed class ThenElse<T> : ICondition<T>
	{
		readonly Func<T, bool> _if, _subject, _else;

		public ThenElse(Func<T, bool> @if, Func<T, bool> subject, Func<T, bool> @else)
		{
			_if      = @if;
			_subject = subject;
			_else    = @else;
		}

		public bool Get(T parameter) => _if(parameter) ? _subject(parameter) : _else(parameter);
	}
}