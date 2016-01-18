using DragonSpark.Extensions;
using PostSharp.Patterns.Threading;
using System;

namespace DragonSpark
{
	[ReaderWriterSynchronized]
	public class ConditionMonitor
	{
		public bool IsApplied => State > ConditionMonitorState.None;

		public ConditionMonitorState State { get; private set; }

		[Writer]
		public void Reset() => State = ConditionMonitorState.None;

		[Writer]
		public bool Apply() => ApplyIf( null, null );

		[Writer]
		public bool Apply( Action action ) => ApplyIf( null, action );

		[Writer]
		public bool ApplyIf( Func<bool> condition, Action action )
		{
			switch ( State )
			{
				case ConditionMonitorState.None:
					State = ConditionMonitorState.Applying;
					var updated = condition.With( item => item(), () => true );
					updated.IsTrue( action );
					State = updated ? ConditionMonitorState.Applied : ConditionMonitorState.None;
					return updated;
			}
			return false;
		}
	}
}