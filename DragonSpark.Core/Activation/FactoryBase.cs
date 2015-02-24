using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation
{
	public class Factory<TResult> : FactoryBase<TResult> where TResult : class
	{
		protected override TResult CreateFrom( object parameter )
		{
			var result = Activator.Create<TResult>( parameter );
			return result;
		}
	}

	public abstract class FactoryBase<TResult> : FactoryBase<object, TResult> where TResult : class
	{}

	public abstract class FactoryBase<TParameter, TResult> : IFactory where TResult : class
	{
		protected abstract TResult CreateFrom( TParameter parameter );
		
		public TResult Create( TParameter parameter = default(TParameter) )
		{
			var result = CreateFrom( parameter );
			return result;
		}


		object IFactory.Create( Type resultType, object parameter )
		{
			var result = Create( parameter.To<TParameter>() );
			return result;
		}
	}
}