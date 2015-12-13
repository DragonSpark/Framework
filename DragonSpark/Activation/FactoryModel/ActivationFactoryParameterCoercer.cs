using System;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationFactoryParameterCoercer<TParameter, TResult> : FactoryParameterCoercer<TParameter>
	{
		protected ActivationFactoryParameterCoercer()
		{}

		protected ActivationFactoryParameterCoercer( IActivator activator ) : base( activator )
		{}

		protected override TParameter PerformCoercion( object context )
		{
			// var type = context.AsTo<Type, Type>( t => typeof(TResult).Extend().IsAssignableFrom( t ) ? t : null ) ?? typeof(TResult);
			var result = Create( context as Type ?? typeof(TResult), context is Type ? null : context );
			return result;
			/*var result = With( p => p, () => CreateDefault( context ) );
			return result;*/
		}

		protected abstract TParameter Create( Type type, object parameter );
	}
}