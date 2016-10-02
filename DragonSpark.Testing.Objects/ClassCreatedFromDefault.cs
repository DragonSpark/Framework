using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Testing.Objects
{
	public class ClassCreatedFromDefault
	{
		readonly static ICache<Type, int> Property = new DecoratedSourceCache<Type, int>();

		public ClassCreatedFromDefault( string message )
		{
			var instance = GetType();
			switch ( Property.Get( instance ) )
			{
				case 0:
					Property.Set( instance, 1 );
					throw new InvalidOperationException( message );
				default:
					Message = message;
					Property.Remove( instance );
					break;
			}
		}

		public string Message { get; private set; }
	}
}