using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Commands;

public class ConditionalCommand<T> : ICommand<T>
{
	readonly ICondition<T> _condition;
	readonly ICommand<T>   _false;
	readonly ICommand<T>   _true;

	public ConditionalCommand(ICondition<T> condition, ICommand<T> @true, ICommand<T> @false)
	{
		_condition = condition;
		_true      = @true;
		_false     = @false;
	}

	public void Execute(T parameter)
	{
		var command = _condition.Get(parameter) ? _true : _false;
		command.Execute(parameter);
	}
}