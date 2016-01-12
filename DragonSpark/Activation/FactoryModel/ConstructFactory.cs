using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactory<TResult> : ActivationFactory<ConstructParameter, TResult> where TResult : class
	{
		public ConstructFactory() : this( () => SystemActivator.Instance )
		{}

		public ConstructFactory( Activator.Get activator ) : this( activator, new ConstructFactoryParameterCoercer<TResult>( activator ) )
		{}

		public ConstructFactory( Activator.Get activator, IFactoryParameterCoercer<ConstructParameter> coercer ) : base( activator, coercer )
		{}

		protected override TResult Activate( ConstructParameter parameter ) => Activator.Construct<TResult>( parameter.Type, parameter.Arguments );
	}

	public abstract class ActivateFactory<TParameter, TResult> : ActivationFactory<TParameter, TResult> where TResult : class where TParameter : ActivateParameter
	{
		protected ActivateFactory( Activator.Get activator ) : base( activator ) {}

		protected ActivateFactory( Activator.Get activator, IFactoryParameterCoercer<TParameter> coercer ) : base( activator, coercer ) {}

		protected override TResult Activate( TParameter parameter ) => Activator.Activate<TResult>( parameter.Type, parameter.Name );
	}
}