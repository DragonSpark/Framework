using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Tasks
{
	public sealed class TaskMonitorContext : SuppliedSource<ISetup, ITaskMonitor>
	{
		public static TaskMonitorContext Default { get; } = new TaskMonitorContext();
		TaskMonitorContext() : base( TaskMonitors.Default.Get, DeclarativeSetup.Current ) {}
	}
}