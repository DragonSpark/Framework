using System;

namespace DragonSpark.Extensions
{
	public static class BooleanExtensions
	{
		public static bool And( this bool target, bool other ) => target && other;

		public static void IsTrue( this bool target, Action action )
		{
			if ( target )
			{
				action?.Invoke();
			}
		}

		public static void IsFalse( this bool target, Action action )
		{
			if ( !target )
			{
				action?.Invoke();
			}
		}
	}
}