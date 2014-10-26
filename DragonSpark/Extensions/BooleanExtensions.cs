using System;

namespace DragonSpark.Extensions
{
	public static class BooleanExtensions
	{
		public static bool And( this bool target, bool other )
		{
			var result = target && other;
			return result;
		}

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