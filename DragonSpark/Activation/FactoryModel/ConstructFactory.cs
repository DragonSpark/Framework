using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactory<TResult> : ActivationFactory<ConstructParameter, TResult> where TResult : class
	{
		public ConstructFactory() : this( () => SystemActivator.Instance ) {}

		public ConstructFactory( Activator.Get activator ) : this( activator, new ConstructFactoryParameterCoercer<TResult>( activator ) ) {}

		public ConstructFactory( Activator.Get activator, IFactoryParameterCoercer<ConstructParameter> coercer ) : base( activator, coercer ) {}

		protected override TResult Activate( ConstructParameter parameter ) => Activator.Construct<TResult>( parameter.Type, parameter.Arguments );
	}
}