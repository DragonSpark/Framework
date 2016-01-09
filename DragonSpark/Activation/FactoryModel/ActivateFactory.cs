using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateFactory<TResult> : ActivateFactory<ActivateParameter, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory() : this( Activation.Activator.GetCurrent ) {}

		public ActivateFactory( Func<IActivator> activator ) : this( activator, new ActivateFactoryParameterCoercer<TResult>( activator ) ) {}

		public ActivateFactory( Func<IActivator> activator, IFactoryParameterCoercer<ActivateParameter> coercer ) : base( activator, coercer ) {}
	}
}