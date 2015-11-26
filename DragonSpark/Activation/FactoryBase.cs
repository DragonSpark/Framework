using System;

namespace DragonSpark.Activation
{
	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult> where TResult : class
	{
		protected abstract TResult CreateItem( TParameter parameter );

		public TResult Create( TParameter parameter )
		{
			var result = CreateItem( parameter );
			return result;
		}

		object IFactoryWithParameter.Create( object parameter )
		{
			var result = Create( parameter is TParameter ? (TParameter)parameter : default(TParameter) );
			return result;
		}

		// Type IFactoryWithParameter.ParameterType => typeof(TParameter);
	}

	public abstract class FactoryBase<TResult> : IFactory<TResult> where TResult : class
	{
		protected abstract TResult CreateItem();

		public TResult Create()
		{
			return CreateItem();
		}

		object IFactory.Create()
		{
			var result = CreateItem();
			return result;
		}
	}
}