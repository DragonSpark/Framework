using System;

namespace DragonSpark.Extensions
{
	public static class WeakReferenceListExtensions
	{
		public static T Get<T>( this WeakReference<T> @this ) where T : class
		{
			T result;
			return @this.TryGetTarget( out result ) ? result : null;
		}
	}
}