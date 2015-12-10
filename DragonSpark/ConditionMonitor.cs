using DragonSpark.Extensions;
using System;

namespace DragonSpark
{
	public class ConditionMonitor
	{
		public bool IsApplied => State > ConditionMonitorState.None;

		public ConditionMonitorState State { get; private set; }

		public void Reset()
		{
			State = ConditionMonitorState.None;
		}

		public bool Apply()
		{
			var result = State == ConditionMonitorState.None;
			State = result ? ConditionMonitorState.Applied : State;
			return result;
		}

		public bool Apply( Action action )
		{
			return ApplyIf( null, action );
		}

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