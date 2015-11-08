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
}