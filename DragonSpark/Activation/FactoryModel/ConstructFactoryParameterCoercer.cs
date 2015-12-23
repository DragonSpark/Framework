using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactoryParameterCoercer<TResult> : ActivationFactoryParameterCoercer<ConstructParameter, TResult>
	{
		public ConstructFactoryParameterCoercer() : this( SystemActivator.Instance )
		{}

		public ConstructFactoryParameterCoercer( IActivator activator ) : base( activator )
		{}

		protected override ConstructParameter Create( Type type, object parameter )
		{
			return new ConstructParameter( type, parameter );
		}
	}
}