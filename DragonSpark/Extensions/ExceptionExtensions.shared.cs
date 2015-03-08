using System;

namespace DragonSpark.Extensions
{
	public static class ExceptionExtensions
	{
		public static void Throw( this Exception target )
		{
			if ( target != null )
			{
				throw target;
			}
		}
	}
}