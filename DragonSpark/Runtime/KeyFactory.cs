using System.Collections;
using System.Collections.Immutable;

namespace DragonSpark.Runtime
{
	public sealed class KeyFactory //  : KeyFactory<int>
	{
		public static KeyFactory Default { get; } = new KeyFactory();
		KeyFactory() {}

		public static int CreateUsing( params object[] parameter ) => Create( ImmutableArray.CreateRange( parameter ) );

		public static int Create( ImmutableArray<object> parameter ) => Hash.CombineValues( Expand( parameter ) );

		static ImmutableArray<object> Expand( ImmutableArray<object> current )
		{
			var builder = current.ToBuilder();
			foreach ( var o in current )
			{
				var list = o as IList;
				if ( list != null )
				{
					builder.Remove( o );
					builder.AddRange( Expand( Cast( list ) ) );
				}
			}
			var result = builder.ToImmutable();
			return result;
		}

		static ImmutableArray<object> Cast( IList source )
		{
			var builder = ImmutableArray.CreateBuilder<object>();
			for ( var i = 0; i < source.Count; i++ )
			{
				builder.Add( source[i] );
			}
			var result = builder.ToImmutable();
			return result;
		}
	}
}