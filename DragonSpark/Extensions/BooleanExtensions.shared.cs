using System;

namespace DragonSpark.Extensions
{
	public static class BooleanExtensions
	{
		public static void IsTrue( this bool target, Action action )
		{
			if ( target && action != null )
			{
				action();
			}
		}

		public static void IsFalse( this bool target, Action action )
		{
			if ( !target && action != null )
			{
				action();
			}
		}
	}
}