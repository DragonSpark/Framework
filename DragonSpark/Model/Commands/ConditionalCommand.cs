using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;

namespace DragonSpark.Model.Commands;

[UsedImplicitly]
public class ConditionalCommand<T> : ICommand<T>
{
	readonly ICondition<T> _condition;
	readonly ICommand<T>   _true, _false;

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