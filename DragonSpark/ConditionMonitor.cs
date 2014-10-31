using DragonSpark.Extensions;
using System;

namespace DragonSpark
{
	public class ConditionMonitor
	{
		bool? applied;

		public bool Applied
		{
			get { return applied.GetValueOrDefault(); }
		}

		public void Reset()
		{
			applied = null;
		}

		public bool Apply()
		{
			var result = !Applied && (bool)( applied = true );
			return result;
		}

		public bool Apply( Action action )
		{
			return ApplyIf( null, action );
		}

		public bool ApplyIf( Func<bool> condition, Action action )
		{
			var current = applied.HasValue;
			if ( !current )
			{
				applied = false;
				var updated = condition.Transform( item => item(), () => true );
				updated.IsTrue( action );
				applied = updated ? true : (bool?)null;
			}
			var result = !current && Applied;
			return result;
		}
	}
}