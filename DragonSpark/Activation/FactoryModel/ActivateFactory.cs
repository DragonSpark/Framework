using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateFactory<TResult> : ActivateFactory<ActivateParameter, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory() : this( Activation.Activator.GetCurrent ) {}
		
		protected ActivateFactory( Activator.Get activator ) : this( activator, new ActivateFactoryParameterCoercer<TResult>( activator ) ) {}

		// protected ActivateFactory( IFactoryParameterCoercer<ActivateParameter> coercer ) : this( Activation.Activator.GetCurrent, coercer ) {}

		protected ActivateFactory( Activator.Get activator, IFactoryParameterCoercer<ActivateParameter> coercer ) : base( activator, coercer ) {}
	}
}