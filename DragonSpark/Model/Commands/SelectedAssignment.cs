using System;

namespace DragonSpark.Model.Commands;

public class SelectedAssignment<TFrom, TTo, T> : ICommand<(TFrom, T)>
{
	readonly Func<TFrom, TTo> _select;
	readonly Action<TTo, T>   _assign;

	public SelectedAssignment(Func<TFrom, TTo> select, Action<TTo, T> assign)
	{
		_select = select;
		_assign = assign;
	}

	public void Execute((TFrom, T) parameter)
	{
		_assign(_select(parameter.Item1), parameter.Item2);
	}
}