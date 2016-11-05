using DragonSpark.Commands;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Tasks
{
	public class MonitorTaskCommand : CommandBase<Task>
	{
		public static MonitorTaskCommand Default { get; } = new MonitorTaskCommand();
		MonitorTaskCommand() : this( TaskMonitorContext.Default.Get ) {}

		readonly Func<ITaskMonitor> monitorSource;

		[UsedImplicitly]
		public MonitorTaskCommand( Func<ITaskMonitor> monitorSource )
		{
			this.monitorSource = monitorSource;
		}

		public override void Execute( Task parameter ) => monitorSource().Monitor( parameter );
	}
}
