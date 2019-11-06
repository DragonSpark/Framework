using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Commands
{
	class ValidatedCommand<T> : ICommand<T>
	{
		readonly ICommand<T>   _command;
		readonly ICondition<T> _condition;

		public ValidatedCommand(ICondition<T> condition, ICommand<T> command)
		{
			_condition = condition;
			_command   = command;
		}

		public void Execute(T parameter)
		{
			if (_condition.Get(parameter))
			{
				_command.Execute(parameter);
			}
		}
	}
}