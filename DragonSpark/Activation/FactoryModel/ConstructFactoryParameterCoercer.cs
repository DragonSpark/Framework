using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactoryParameterCoercer<TResult> : ActivationFactoryParameterCoercer<ConstructParameter, TResult>
	{
		public ConstructFactoryParameterCoercer( IActivator activator ) : base( activator )
		{}

		protected override ConstructParameter Create( Type type, object parameter )
		{
			return new ConstructParameter( type, parameter );
		}
	}

	public class ActivateFactoryParameterCoercer<TResult> : ActivationFactoryParameterCoercer<ActivateParameter, TResult>
	{
		public ActivateFactoryParameterCoercer( IActivator activator ) : base( activator )
		{}

		protected override ActivateParameter Create( Type type, object parameter )
		{
			return new ActivateParameter( type, parameter.As<string>() );
		}
	}
}