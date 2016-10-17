using System;

namespace DragonSpark.Runtime
{
	public interface IComposable<in T>
	{
		void Add( T instance );
	}

	public static class Extensions
	{
		public static T Registered<T>( this IComposable<IDisposable> @this, T entry ) where T : IDisposable
		{
			@this.Add( entry );
			return entry;
		}
	}
}