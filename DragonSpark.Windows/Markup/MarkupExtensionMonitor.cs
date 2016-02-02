using DragonSpark.Runtime.Values;
using System;

namespace DragonSpark.Windows.Markup
{
	public class MarkupExtensionMonitor
	{
		public event EventHandler Initialized = delegate {};

		readonly ConditionMonitor monitor = new ConditionMonitor();

		public bool IsInitialized => monitor.State != ConditionMonitorState.None;

		public void Initialize() => monitor.Apply( () =>
		{
			Initialized( this, EventArgs.Empty );
			Initialized = delegate { };
		} );
	}

	public class AssociatedMonitor : AssociatedValue<MarkupExtensionMonitor>
	{
		public AssociatedMonitor( object instance ) : base( instance, () => new MarkupExtensionMonitor() ) {}
	}
}