using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Tasks
{
	public sealed class TaskMonitors : Cache<ISetup, ITaskMonitor>
	{
		public static ITaskMonitor Current() => Default.Get( DeclarativeSetup.Current() );

		public static TaskMonitors Default { get; } = new TaskMonitors();
		TaskMonitors() : base( setup => new TaskMonitor() ) {}
	}
}