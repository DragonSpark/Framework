using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Model.Commands;

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