using System;

namespace DragonSpark.Windows.Markup
{
	public class MarkupExtensionMonitor
	{
		public static MarkupExtensionMonitor Instance { get; } = new MarkupExtensionMonitor();

		readonly ConditionMonitor monitor = new ConditionMonitor();
		
		public event EventHandler Initialized = delegate { };

		MarkupExtensionMonitor()
		{}

		public bool IsInitialized => monitor.State != ConditionMonitorState.None;

		public void Initialize()
		{
			monitor.Apply( () =>
			{
				Initialized( this, EventArgs.Empty );
				Initialized = delegate { };
			} );
		}
	}
}