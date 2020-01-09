using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model
{
	public class Guard<T, TException> : ICommand<T> where TException : Exception
	{
		readonly Func<T, bool>       _condition;
		readonly Func<T, TException> _exception;

		public Guard(ICondition<T> condition, Func<T, string> message) : this(condition.Get, message) {}

		public Guard(Func<T, bool> condition, Func<T, string> message)
			: this(condition, Start.A.Selection<string>()
			                       .AndOf<TException>()
			                       .By.Instantiation.To(message.Start().Select)) {}

		public Guard(Func<T, bool> condition, Func<T, TException> exception)
		{
			_condition = condition;
			_exception = exception;
		}

		public void Execute(T parameter)
		{
			var condition = _condition(parameter);
			if (condition)
			{
				throw _exception(parameter);
			}
		}
	}
}