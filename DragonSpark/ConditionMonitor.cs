using DragonSpark.Extensions;
using System;

namespace DragonSpark
{
	public class ConditionMonitor
	{
		public bool Applied { get; private set; }

		public void Reset()
		{
			Applied = false;
		}

		public bool Apply()
		{
			var result = !Applied && ( Applied = true );
			return result;
		}

		public bool Apply( Action action )
		{
			return ApplyIf( null, action );
		}

		public bool ApplyIf( Func<bool> condition, Action action )
		{
			var original = Applied;
			if ( !Applied )
			{
				Applied = condition.Transform( item => item(), () => true );
				Applied.IsTrue( action );
			}
			var result = !original && Applied;
			return result;
		}
	}
}