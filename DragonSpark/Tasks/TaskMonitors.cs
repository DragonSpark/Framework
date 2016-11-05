using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized.Caching;
using JetBrains.Annotations;

namespace DragonSpark.Tasks
{
	public sealed class TaskMonitors : Cache<ITaskMonitor>
	{
		[UsedImplicitly]
		public static TaskMonitors Default { get; } = new TaskMonitors();
		TaskMonitors() : base( x => Disposables.Default.Registered( new TaskMonitor() ) ) {}
	}
}