using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;
using System.Reflection;

namespace DragonSpark.Activation
{
	public class Factory<TResult> : Factory<object, TResult> where TResult : class
	{}

	public class Factory<TParameter, TResult> : IFactory where TResult : class
	{
		protected virtual TResult CreateFrom( TParameter parameter )
		{
			var result = FromLocation( parameter ) ?? FromActivation( parameter );
			return result;
		}

		protected virtual TResult FromLocation( TParameter parameter )
		{
			var result = ServiceLocation.Locate<TResult>();
			return result;
		}

		protected virtual TResult FromActivation( TParameter parameter )
		{
			var result = System.Activator.CreateInstance<TResult>();
			return result;
		}

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