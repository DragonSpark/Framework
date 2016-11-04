using System;

namespace DragonSpark.Runtime
{
	public static class Extensions
	{
		public static T Registered<T>( this IComposable<IDisposable> @this, T entry ) where T : IDisposable
		{
			@this.Add( entry );
			return entry;
		}

		public static T Registered<T>( this IComposable<object> @this, T entry ) 
		{
			@this.Add( entry );
			return entry;
		}
	}
}