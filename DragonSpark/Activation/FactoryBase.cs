using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation
{
	public interface IFactory<in TParameter, out TResult>
	{
		TResult Create( TParameter parameter );
	}

	public abstract class FactoryBase<TResult> : FactoryBase<object, TResult> where TResult : class
	{}

	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult>, IFactory where TResult : class
	{
		protected virtual Type ResultType => typeof(TResult);

		protected abstract TResult CreateFrom( Type resultType, TParameter parameter );

		public TResult Create()
		{
			return Create( default(TParameter) );
		}

		public TResult Create( TParameter parameter )
		{
			return Create( ResultType, parameter );
		}

		public TResult Create( Type resultType, TParameter parameter )
		{
			// var type = resultType == typeof(object) || resultType == null ? ResultType : resultType;
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