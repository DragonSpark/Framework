using DragonSpark.Application.Setup;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized.Caching;
using JetBrains.Annotations;

namespace DragonSpark.Tasks
{
	public sealed class TaskMonitors : Cache<ISetup, ITaskMonitor>
	{
		public static ITaskMonitor Current() => Default.Get( DeclarativeSetup.Current() );

		[UsedImplicitly]
		public static TaskMonitors Default { get; } = new TaskMonitors();
		TaskMonitors() : base( setup => Disposables.Default.Registered( new TaskMonitor() ) ) {}
	}
}