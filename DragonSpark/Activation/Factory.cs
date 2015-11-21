using System;

namespace DragonSpark.Activation
{
	public class Factory<TResult> : FactoryBase<TResult> where TResult : class
	{
		readonly IActivator activator;

		public Factory() : this( SystemActivator.Instance )
		{}

		public Factory( IActivator activator )
		{
			this.activator = activator;
		}

		protected override TResult CreateFrom( Type resultType, object parameter )
		{
			var result = activator.Construct<TResult>( parameter );
			return result;
		}
	}
}