using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model.Commands;

public class ValidatedCommand : ValidatedCommand<None>
{
	public ValidatedCommand(ICondition condition, ICommand command) : base(condition, command) {}
}

public class ValidatedCommand<T> : ICommand<T>
{
	readonly Action<T>     _command;
	readonly Func<T, bool> _condition;

	public ValidatedCommand(ICondition<T> condition, ICommand<T> command)
		: this(condition.Get, command.Execute) {}

	public ValidatedCommand(Func<T, bool> condition, Action<T> command)
	{
		_condition = condition;
		_command   = command;
	}

	public void Execute(T parameter)
	{
		if (_condition(parameter))
		{
			_command(parameter);
		}
	}
}