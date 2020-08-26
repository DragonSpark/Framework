using DragonSpark.Compose;
using DragonSpark.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	sealed class Operations : IOperations
	{
		readonly IList<Task> _tasks;

		public Operations() : this(new List<Task>()) {}

		public Operations(IList<Task> tasks) => _tasks = tasks;

		public void Execute(Task parameter)
		{
			_tasks.Add(parameter);
		}

		public ValueTask Get() => Task.WhenAll(_tasks).ToOperation();

		public void Execute(None parameter)
		{
			_tasks.Clear();
		}

		public bool Get(None parameter) => _tasks.Count > 0;
	}
}