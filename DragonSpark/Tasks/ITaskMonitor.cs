using System;
using System.Threading.Tasks;

namespace DragonSpark.Tasks
{
	public interface ITaskMonitor : IDisposable
	{
		void Monitor( Task task );
	}
}