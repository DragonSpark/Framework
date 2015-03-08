using System;
using DragonSpark.Extensions;

namespace DragonSpark.Objects
{
	public static class FactoryExtensions
	{
		public static TResult Create<TResult>( this IFactory target, object source = null )
		{
			var result = target.Create( typeof(TResult), source ).To<TResult>();
			return result;
		}
	}

	public interface IFactory
	{
		object Create( Type resultType, object source = null );
	}
}