using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;

namespace DragonSpark.Aspects
{
	public abstract class AdapterLocatorBase<T> : AdapterLocatorBase<object, T>
	{
		protected AdapterLocatorBase( IEnumerable<ValueTuple<TypeAdapter, Func<object, T>>> pairs ) : base( pairs ) {}
	}

	public abstract class AdapterLocatorBase<TParameter, TResult> : IParameterizedSource<TParameter, TResult>
	{
		readonly IEnumerable<ValueTuple<TypeAdapter, Func<TParameter, TResult>>> pairs;

		protected AdapterLocatorBase( IEnumerable<ValueTuple<TypeAdapter, Func<TParameter, TResult>>> pairs )
		{
			this.pairs = pairs;
		}

		public TResult Get( TParameter parameter )
		{
			var type = TypeSupport.From( parameter );
			foreach ( var pair in pairs )
			{
				if ( pair.Item1.IsAssignableFrom( type ) )
				{
					var result = pair.Item2( parameter );
					if ( result != null )
					{
						return result;
					}
				}
			}

			throw new InvalidOperationException( $"{typeof(TResult).FullName} not found for {type}." );
		}
	}
}