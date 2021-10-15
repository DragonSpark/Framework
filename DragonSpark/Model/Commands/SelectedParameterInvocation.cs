using System;

namespace DragonSpark.Model.Commands;

public class SelectedParameterInvocation<TFrom, TTo, _> : ICommand<TFrom>
{
	readonly Func<TFrom, TTo> _select;
	readonly Func<TTo, _>     _source;

	public SelectedParameterInvocation(Func<TTo, _> source, Func<TFrom, TTo> select)
	{
		_select = select;
		_source = source;
	}

	public void Execute(TFrom parameter)
	{
		_source(_select(parameter));
	}
}