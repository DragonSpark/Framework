using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation
{
	public class Factory<TResult> : FactoryBase<TResult> where TResult : class
	{
		protected override TResult CreateFrom( Type resultType, object parameter )
		{
			var result = Activator.Create<TResult>( parameter );
			return result;
		}
	}

	public abstract class FactoryBase<TResult> : FactoryBase<object, TResult> where TResult : class
	{}

	public abstract class FactoryBase<TParameter, TResult> : IFactory where TResult : class
	{
		protected abstract TResult CreateFrom( Type resultType, TParameter parameter );
		
		public TResult Create( Type resultType, TParameter parameter = default(TParameter) )
		{
			var result = CreateFrom( resultType, parameter );
			return result;
		}


		object IFactory.Create( Type resultType, object parameter )
		{
			var result = Create( resultType, parameter.To<TParameter>() );
			return result;
		}
	}
}