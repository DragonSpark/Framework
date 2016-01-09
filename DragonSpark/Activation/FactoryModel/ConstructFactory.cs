using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactory<TResult> : ActivationFactory<ConstructParameter, TResult> where TResult : class
	{
		public ConstructFactory() : this( () => SystemActivator.Instance )
		{}

		public ConstructFactory( Func<IActivator> activator ) : this( activator, new ConstructFactoryParameterCoercer<TResult>( activator ) )
		{}

		public ConstructFactory( Func<IActivator> activator, IFactoryParameterCoercer<ConstructParameter> coercer ) : base( activator, coercer )
		{}

		protected override TResult Activate( ConstructParameter parameter ) => Activator.Construct<TResult>( parameter.Type, parameter.Arguments );
	}

	public abstract class ActivateFactory<TParameter, TResult> : ActivationFactory<TParameter, TResult> where TResult : class where TParameter : ActivateParameter
	{
		protected ActivateFactory( Func<IActivator> activator ) : base( activator ) {}

		protected ActivateFactory( Func<IActivator> activator, IFactoryParameterCoercer<TParameter> coercer ) : base( activator, coercer ) {}

		protected override TResult Activate( TParameter parameter ) => Activator.Activate<TResult>( parameter.Type, parameter.Name );
	}
}