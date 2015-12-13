using System;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationFactoryParameterCoercer<TParameter, TResult> : FactoryParameterCoercer<TParameter>
	{
		protected ActivationFactoryParameterCoercer( IActivator activator ) : base( activator )
		{}

		protected override TParameter PerformCoercion( object context )
		{
			var result = Create( context as Type ?? typeof(TResult), context is Type ? null : context );
			return result;
		}

		protected abstract TParameter Create( Type type, object parameter );
	}
}