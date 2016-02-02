using System;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Windows.Markup
{
	public class MarkupExtensionMonitor
	{
		// public static MarkupExtensionMonitor Instance { get; } = new MarkupExtensionMonitor();

		readonly ConditionMonitor monitor = new ConditionMonitor();
		
		public event EventHandler Initialized = delegate { };

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

	public class AssociatedMonitor : AssociatedValue<MarkupExtensionMonitor>
	{
		public AssociatedMonitor( object instance ) : base( instance, () => new MarkupExtensionMonitor() ) {}
	}
}