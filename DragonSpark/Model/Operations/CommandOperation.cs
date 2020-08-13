using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class CommandOperation<T> : IOperation<T>
	{
		readonly Action<T> _action;

		public CommandOperation(Action<T> action) => _action = action;

		public ValueTask Get(T parameter)
		{
			_action(parameter);
			return Task.CompletedTask.ToOperation();
		}
	}
}
