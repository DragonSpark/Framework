using System;

namespace DragonSpark.Diagnostics.Logging
{
	public class SelectedLogException<TIn, T> : ILogException<TIn>
	{
		readonly Func<TIn, T>     _select;
		readonly ILogException<T> _previous;

		public SelectedLogException(Func<TIn, T> select, ILogException<T> previous)
		{
			_select   = @select;
			_previous = previous;
		}

		public void Execute(ExceptionParameter<TIn> parameter)
		{
			var (exception, argument) = parameter;
			_previous.Execute(new (exception, _select(argument)));
		}
	}
}