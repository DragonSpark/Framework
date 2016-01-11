using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructFactoryParameterCoercer<TResult> : ActivationFactoryParameterCoercer<ConstructParameter, TResult>
	{
		public ConstructFactoryParameterCoercer() : this( Activator.GetCurrent )
		{}

		public ConstructFactoryParameterCoercer( Func<IActivator> activator ) : base( activator )
		{}

		protected override ConstructParameter Create( Type type, object parameter ) => new ConstructParameter( type, parameter );
	}
}