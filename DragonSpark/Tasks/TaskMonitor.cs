using DragonSpark.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Tasks
{
	public class TaskMonitor : DisposableBase, ITaskMonitor
	{
		readonly ICollection<Task> tasks = new List<Task>();

		public void Monitor( Task task ) => tasks.Add( task );

		protected override void OnDispose( bool disposing )
		{
			Task.WhenAll( tasks ).Wait();
			tasks.Clear();
		}
	}
}