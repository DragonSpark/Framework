using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Commands
{
	public class SelectedParameterCommand<TFrom, TTo> : ICommand<TFrom>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Action<TTo>      _source;

		public SelectedParameterCommand(Action<TTo> source, Func<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		public void Execute(TFrom parameter) => _source(_select(parameter));
	}

	public class SelectedCommand<T> : ICommand<T>
	{
		readonly Func<T, ICommand<T>> _select;

		public SelectedCommand(ISelect<T, ICommand<T>> select) : this(select.Get) {}

		public SelectedCommand(Func<T, ICommand<T>> select) => _select = select;

		public void Execute(T parameter)
		{
			_select(parameter).Execute(parameter);
		}
	}
}