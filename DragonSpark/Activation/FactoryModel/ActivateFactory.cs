using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateFactory<TResult> : ActivationFactory<ActivateParameter, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory() : this( Activation.Activator.GetCurrent ) {}
		
		ActivateFactory( Activator.Get activator ) : this( activator, new ActivateFactoryParameterCoercer<TResult>( activator ) ) {}

		ActivateFactory( Activator.Get activator, IFactoryParameterCoercer<ActivateParameter> coercer ) : base( activator, coercer ) {}

		protected override TResult Activate( ActivateParameter parameter ) => Activator.Activate<TResult>( parameter.Type, parameter.Name );
	}
}