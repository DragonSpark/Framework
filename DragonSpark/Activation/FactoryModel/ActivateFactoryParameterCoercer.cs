using System;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateFactoryParameterCoercer<TResult> : ActivationFactoryParameterCoercer<ActivateParameter, TResult>
	{
		public ActivateFactoryParameterCoercer() : this( Activator.GetCurrent ) {}

		public ActivateFactoryParameterCoercer( Activator.Get activator ) : base( activator ) {}

		protected override ActivateParameter Create( Type type, object parameter ) => new ActivateParameter( type, parameter.As<string>() );
	}
}