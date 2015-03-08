using DragonSpark.Extensions;
using System.Linq;

namespace DragonSpark.Objects.Synchronization
{
	public static class ObjectExtensions
	{
/*
		public static object Evaluate( this object target, string expression )
		{
			var result = Evaluate<object>( target, expression );
			return result;
		}

		public static TResult Evaluate<TResult>( this object target, string expression )
		{
			var value = Expression.Evaluate( target, expression ).Last.Value.Value;
			var result = value.To<TResult>();
			return result;
		}
*/

		public static TResult SynchronizeFrom<TResult>( this TResult target, object source, params string[] propertiesToIgnore )
		{
			var key = new SynchronizationKey( target.GetType(), source.GetType() );
			var policy = new SynchronizationPolicy( key, new SimilarProperties( propertiesToIgnore ) );
			var result = SynchronizeFrom( target, source, policy.ToEnumerable().ToArray() );
			return result;
		}

		public static TResult SynchronizeFrom<TResult>( this TResult target, object source, ISynchronizationPolicy[] policies )
		{
			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target, true );
			return target;
		}
	}
}
