using DragonSpark.Extensions;
using DragonSpark.Setup;
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

		protected override TResult CreateFrom( object parameter )
		{
			var result = activator.Construct<TResult>( parameter );
			return result;
		}
	}

	public class ActivateFactory<TResult> : FactoryBase<Type, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory() : this( SystemActivator.Instance )
		{}

		public ActivateFactory( IActivator activator )
		{
			Activator = activator;
		}

		public IActivator Activator { get; }

		protected override TResult CreateFrom( Type parameter )
		{
			var type = parameter.Extend().GuardAsAssignable<ISetup>( nameof(parameter) );
			var result = Activator.Activate<TResult>( type );
			return result;
		}
	}
}