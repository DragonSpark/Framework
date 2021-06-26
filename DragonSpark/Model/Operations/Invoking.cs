using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Invoking<T> : IOperation<T>
	{
		readonly Action<T> _action;

		public Invoking(Action action) : this(Start.A.Command(action).Accept<T>()) {}

		public Invoking(Action<T> action) => _action = action;

		public ValueTask Get(T parameter)
		{
			_action(parameter);
			return Task.CompletedTask.ToOperation();
		}
	}
}