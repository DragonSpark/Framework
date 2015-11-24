using DragonSpark.Runtime;
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

		protected override TResult CreateFrom( Type resultType, object parameter )
		{
			var result = activator.Construct<TResult>( parameter );
			return result;
		}
	}

	public class ActivateFactory<TResult> : FactoryBase<TypeExtension, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		readonly IActivator activator;

		public ActivateFactory() : this( SystemActivator.Instance )
		{}

		public ActivateFactory( IActivator activator )
		{
			this.activator = activator;
		}

		protected override TResult CreateFrom( Type resultType, TypeExtension parameter )
		{
			var type = parameter.GuardAsAssignable<ISetup>( nameof(parameter) );
			var result = activator.Activate<TResult>( type );
			return result;
		}
	}
}